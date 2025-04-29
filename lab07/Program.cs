using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

class Program
{
    const string PublicKeyPath = "rsa_public.xml";
    const string PrivateKeyPath = "rsa_private.xml";

    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            ShowHelp();
            return;
        }

        try
        {
            string mode = args[0].ToLower();

            switch (mode)
            {
                case "rsa":
                    HandleRSA(args);
                    break;
                case "hash":
                    HandleHash(args);
                    break;
                case "sign":
                    HandleSign(args);
                    break;
                case "aes":
                    HandleAES(args);
                    break;
                default:
                    Console.WriteLine("Nieznany tryb: " + mode);
                    ShowHelp();
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Błąd: " + ex.Message);
        }
    }

    static void ShowHelp()
    {
        Console.WriteLine("Użycie:");
        Console.WriteLine(" RSA:");
        Console.WriteLine("  Program rsa 0");
        Console.WriteLine("  Program rsa 1 <plik_in> <plik_out>");
        Console.WriteLine("  Program rsa 2 <plik_in> <plik_out>");
        Console.WriteLine(" HASH:");
        Console.WriteLine("  Program hash <plik> <plik_hash> <SHA256|SHA512|MD5>");
        Console.WriteLine(" SIGN:");
        Console.WriteLine("  Program sign <plik> <plik_podpisu>");
        Console.WriteLine(" AES:");
        Console.WriteLine("  Program aes <plik_in> <plik_out> <hasło> <0=enc|1=dec>");
    }

    // 1. RSA
    static void HandleRSA(string[] args)
    {
        if (args.Length < 2)
            throw new ArgumentException("Użycie: Program rsa <0|1|2> [plik_we] [plik_wy]");

        int submode = int.Parse(args[1]);

        switch (submode)
        {
            case 0:
                using (var rsa = new RSACryptoServiceProvider(2048))
                {
                    File.WriteAllText(PublicKeyPath, rsa.ToXmlString(false));
                    File.WriteAllText(PrivateKeyPath, rsa.ToXmlString(true));
                    Console.WriteLine("Wygenerowano klucze RSA.");
                }
                break;

            case 1: // Szyfrowanie
                if (args.Length < 4) throw new ArgumentException("Brakuje parametrów: <plik_in> <plik_out>");
                string inputEnc = args[2], outputEnc = args[3];
                string pubKey = File.ReadAllText(PublicKeyPath);
                byte[] data = File.ReadAllBytes(inputEnc);
                using (var rsaEnc = new RSACryptoServiceProvider())
                {
                    rsaEnc.FromXmlString(pubKey);
                    byte[] encrypted = rsaEnc.Encrypt(data, false);
                    File.WriteAllBytes(outputEnc, encrypted);
                    Console.WriteLine("Plik zaszyfrowany.");
                }
                break;

            case 2: // Deszyfrowanie
                if (args.Length < 4) throw new ArgumentException("Brakuje parametrów: <plik_in> <plik_out>");
                string inputDec = args[2], outputDec = args[3];
                string privKey = File.ReadAllText(PrivateKeyPath);
                byte[] encryptedData = File.ReadAllBytes(inputDec);
                using (var rsaDec = new RSACryptoServiceProvider())
                {
                    rsaDec.FromXmlString(privKey);
                    byte[] decrypted = rsaDec.Decrypt(encryptedData, false);
                    File.WriteAllBytes(outputDec, decrypted);
                    Console.WriteLine("Plik odszyfrowany.");
                }
                break;
        }
    }

    // 2. Hash
    static void HandleHash(string[] args)
    {
        if (args.Length < 4)
            throw new ArgumentException("Użycie: Program hash <plik> <plik_hash> <SHA256|SHA512|MD5>");

        string file = args[1], hashFile = args[2], algorithm = args[3].ToUpper();
        byte[] data = File.ReadAllBytes(file);
        byte[] hash = algorithm switch
        {
            "SHA256" => SHA256.Create().ComputeHash(data),
            "SHA512" => SHA512.Create().ComputeHash(data),
            "MD5" => MD5.Create().ComputeHash(data),
            _ => throw new ArgumentException("Nieznany algorytm.")
        };

        if (!File.Exists(hashFile))
        {
            File.WriteAllBytes(hashFile, hash);
            Console.WriteLine("Hash zapisany.");
        }
        else
        {
            byte[] existing = File.ReadAllBytes(hashFile);
            Console.WriteLine(hash.SequenceEqual(existing)
                ? "Hash zgodny."
                : "Hash NIEZGODNY!");
        }
    }

    // 3. RSA podpis cyfrowy
    static void HandleSign(string[] args)
    {
        if (args.Length < 3)
            throw new ArgumentException("Użycie: Program sign <plik> <plik_podpisu>");

        string file = args[1], sigFile = args[2];
        byte[] data = File.ReadAllBytes(file);

        if (!File.Exists(sigFile))
        {
            using var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(File.ReadAllText(PrivateKeyPath));
            byte[] signature = rsa.SignData(data, CryptoConfig.MapNameToOID("SHA256"));
            File.WriteAllBytes(sigFile, signature);
            Console.WriteLine("Podpis wygenerowany.");
        }
        else
        {
            using var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(File.ReadAllText(PublicKeyPath));
            byte[] signature = File.ReadAllBytes(sigFile);
            bool valid = rsa.VerifyData(data, CryptoConfig.MapNameToOID("SHA256"), signature);
            Console.WriteLine(valid ? "Podpis poprawny." : "Podpis NIEPOPRAWNY!");
        }
    }

    // 4. AES z hasłem
    static void HandleAES(string[] args)
    {
        if (args.Length < 5)
            throw new ArgumentException("Użycie: Program aes <plik_in> <plik_out> <hasło> <0|1>");

        string input = args[1], output = args[2], password = args[3];
        int mode = int.Parse(args[4]);

        byte[] salt = Encoding.UTF8.GetBytes("StałaSól1234");
        var key = new Rfc2898DeriveBytes(password, salt, 10000);
        byte[] aesKey = key.GetBytes(32);
        byte[] aesIV = key.GetBytes(16);

        using var aes = Aes.Create();
        aes.Key = aesKey;
        aes.IV = aesIV;

        if (mode == 0)
        {
            using var encryptor = aes.CreateEncryptor();
            byte[] plain = File.ReadAllBytes(input);
            byte[] encrypted = encryptor.TransformFinalBlock(plain, 0, plain.Length);
            File.WriteAllBytes(output, encrypted);
            Console.WriteLine("Plik zaszyfrowany AES.");
        }
        else
        {
            using var decryptor = aes.CreateDecryptor();
            byte[] encrypted = File.ReadAllBytes(input);
            byte[] decrypted = decryptor.TransformFinalBlock(encrypted, 0, encrypted.Length);
            File.WriteAllBytes(output, decrypted);
            Console.WriteLine("Plik odszyfrowany AES.");
        }
    }
}
