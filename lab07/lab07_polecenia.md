# Laboratorium 07: Podstawowe funkcje kryptograficzne
## Programowanie zaawansowane 2

- Maksymalna liczba punktów: 10

- Skala ocen za punkty:
    - 9-10 ~ bardzo dobry (5.0)
    - 8 ~ plus dobry (4.5)
    - 7 ~ dobry (4.0)
    - 6 ~ plus dostateczny (3.5)
    - 5 ~ dostateczny (3.0)
    - 0-4 ~ niedostateczny (2.0)

Celem laboratorium jest zapoznanie z działaniem podstawowych funkcji kryptograficznych w języku C#. Wszystkie programy powinny być obsługiwane przy pomocy parametrów konsoli. Obsłuż sytuacje wynikające z niewłaściwej liczby parametrów, braków wymaganych plików itp. Każdy ewentualny błąd ma być wyświetlany przy pomocy sensownego :-) komunikatu w konsoli. UWAGA! Zakładamy, że mamy na tyle RAM, aby wczytać każdy plik do pamięci (nie musimy strumieniować plików po kawałku). 

1. [3 punkty] Program szyfrujący przy pomocy kryptografii klucza asymetrycznego. Napisz program który jako parametr przyjmuje typ polecenia. W zależności od wybranego typu polecenia:
    - Jeżeli typ polecenia = 0 program ma wygenerować i zapisać do dwóch plików (o dowolnych nazwach, można je wpisać "na sztywno") klucz publiczny oraz klucz prywatny algorytmu RSA.
    - Jeżeli typ polecenia = 1, program dodatkowo pobiera nazwę dwóch plików (a), (b). Podany plik (a) ma zostać zaszyfrowany przy pomocy klucza publicznego odczytanego z pliku, który został stworzony przy pomocy tego programu kiedy typ polecenia = 0. Zaszyfrowane dane mają być zapisane w pliku (b).
    - Jeżeli typ polecenia = 2, program dodatkowo pobiera nazwę dwóch plików (a), (b). Podany plik (a) ma zostać odszyfrowany przy pomocy klucza prywatnego odczytanego z pliku, który został stworzony przy pomocy tego programu kiedy typ polecenia = 0. Odszyfrowane dane mają być zapisane w pliku (b).
2. [2 punkty] Program liczący sumę kontrolną. Napisz program który jako parametry przyjmuje nazwę pliku (a), nazwę pliku zawierającego hash (b) oraz algorytm hashowania (SHA256, SHA512 lub MD5) (c). Jeżeli plik hash (b) nie istnieje, program ma policzyć hash z pliku (a) i zapisać go pod nazwą (b). Jeżeli plik (b) istnieje, program ma zweryfikować hash i wypisać do konsoli, czy hash jest zgodny.
3. [3 punkty] Podpisywanie danych z pliku. Zakładamy, że mamy dwa pliki w których znajduje się klucz prywatny i publiczny algorytmu RSA. Te pliki zostały utworzone np. programem z punktu 1. Program pobiera nazwę dwóch plików (a) i (b). Program wczytuje plik (a). Jeśli plik (b) istnieje, program ma potraktować go jako podpis wygenerowany z pliku (a) przy pomocy klucza prywatnego. Program ma zweryfikować, czy podpis jest poprawny i wypisać wynik weryfikacji na ekran. Jeśli plik (b) nie istnieje, program ma wygenerować podpis danych z pliku (a) używając klucza prywatnego i zapisać ten podpis do pliku (b).
4. [2 punkty] Zaszyfrowanie pliku algorytmem klucza symetrycznego przy użyciu hasła. Program ma przyjmować cztery parametry: pliki (a), (b), hasło, typ operacji. 
    - Jeżeli typ operacji = 0 program ma zaszyfrować plik (a) algorytmem AES, którego klucz ma zostać wygenerowany przy pomocy podanego hasła. Zaszyfrowane dane mają być zapisane do pliku (b).
    - Jeżeli typ operacji = 1 program ma odszyfrować plik (a) algorytmem AES, którego klucz ma zostać wygenerowany przy pomocy podanego hasła. Odszyfrowane dane mają być zapisane do pliku (b).
Wszystkie dane wymagane do utworzenia klucza algorytmu AES z wyjątkiem hasła mogą byc wpisane "na sztywno" do programu.

Powodzenia! (งツ)ว
