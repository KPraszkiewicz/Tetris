# Zadanie

Zadanie testowe: Tetris
- dwóch graczy, jeden gra po lewej drugi po prawej
- minimalistyczny UI: next klocek, score, nazwa gracza
- nie muszą być wszystkie typy klocków wystarczą dwa, trzy żeby widzieć że działa
- klocki przesuwają się płynnie (a nie przeskakują co kratkę) i do ruchu i zderzeń używają fizyki. Aczkolwiek nie powinny się móc obracać.
- Logika na oddzielnej scenie zaś jej kod w oddzielnym assembly
- UI, wczytywanie inputu i prezentacja (klocki) na oddzielnej scenie zaś jego kod w oddzielnym assembly (czyli w sumie 2 assembly i 2 sceny)

# Status
- Można grać w 2 osoby na jednej klawiaturze
- Można przesuwać klocki na boki
- Za każdą wypełnioną linię gracz dostaje 100 punktów
- Poruszanie się klocków i kolizje są oparte na fizyce
- Kod jest podzielony na "assembly"

![tetris_in_game](https://github.com/user-attachments/assets/78e75da5-96c8-46aa-802b-d869e99d1f10)

# Plany
- Dodanie obrotów w systemie SRS (https://tetris.wiki/Super_Rotation_System)
- Poprawienie błędów z kolizją w górnej części planszy
- Fizyka oparta na DOTS (Data-Oriented Technology Stack)
- Możliwość gry przez sieć
- Możliwość rozmowy przez przez czat głosowy
