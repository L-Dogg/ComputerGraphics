# ComputerGraphics

## GK1:
Edytor wielokątów - podstawowa specyfikacja:

* Możliwość dodawania nowego wielokąta, usuwania oraz edycji
* Przy edycji:
  * przesuwanie wierzchołka lub całego wielokąta
  * usuwanie wierzchołka
  * dodawanie wierzchołka w środku wybranej krawędzi
  * opcjonalnie: przesuwanie całej krawędzi
* Dodawanie ograniczeń (relacji) dla wybranej krawędzi:
   * moźliwe ograniczenia: krawędź pozioma, krawędź pionowa, dana długość krawędzi
   * maksymalnie tylko jedno ograniczenie dla krawędzi
   * dwie sąsiednie krawędzie nie mogą być obie pionowe lub obie poziome
   * maksymalnie tylko jedno ograniczenie dla krawędzi
   * opcjonalnie: jeden punkt wielokąta (np. pierwszy wprowadzony) jest "usztywniony". Nie można go przesuwać i usuwać (chyba, że przesuwamy cały wielokąt)
   * dodawanie wierzchołka na krawędzi lub usuwanie wierzchołka - usuwa ograniczenia "przyległych" krawędzi
    ustawione ograniczenia są widoczne (jako odpowiednie "ikonki") przy środku krawędzi
* Rysowanie odcinków - własna implementacja - alg. Bresenhama

## GK2:
Wypełnianie i obcinanie - podstawowa specyfikacja:

* Wypełnianie wybranych wielokątów
  * Algorytm wypełniania wielokątów z sortowaniem krawędzi (kubełkowym)
  * Kolor wypełniania:
    * Składowa rozproszona modelu oświetlenia (model Lamberta) : I = IL*IO*cos(kąt(N,L))
    * IL(kolor światła) - możliwość wyboru z menu
    * IO(kolor obiektu) - wczytywana tekstura (obraz) całego 'panelu'
    * L (wektor do światła) - stały (0,0,1) albo punkt animowany po półsferze wokół ekranu
    * Wektor normalny N:
      * Podawany w teksturze, Nx=<-1,+1>, Ny=<-1,+1>, Nz=<0,+1> (np. N=(0,0,1) => RGB(127,127,255) )
      * 'Zaburzenie' wektora normalnego - mapowanie nierówności (bump mapping) N' = N + D (zaburzenie)
    zaburzenie na podstawie mapy wysokości (height map) odczytywanej z tektury - algorytm Blinna
* "obcinanie" wybranych dwóch wielokątów (z dwóch zaznaczonych powstaje jeden)
 * Część wspólna wielokątów - algorytm Weilera-Athertona - grupa a
