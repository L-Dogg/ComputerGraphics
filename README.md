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
