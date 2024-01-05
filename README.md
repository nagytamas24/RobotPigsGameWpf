# RobotPigsGameWpf
Az Eötvös Loránd Tudományegyetem Eseményvezérelt alkalmazások nevű tárgy egyik beadandó feladatának megoldása.
A játékot MVVM architektúrában, az objektum oriántáltság követelményeinek megfelelően C# nyelven kellett megvalósítani.


A feladat a következő:
Készítsünk programot, amellyel a következő két személyes játékot játszhatjuk. 
Adott egy 𝑛 × 𝑛 elemből álló játékpálya, ahol két harcos robotmalac helyezkedik 
el, kezdetben a két ellentétes oldalon, a középvonaltól eggyel jobbra, és 
mindkettő előre néz. A malacok lézerágyúval és egy támadóököllel vannak 
felszerelve. 
A játék körökből áll, minden körben a játékosok egy programot futtathatnak a 
malacokon, amely öt utasításból állhat (csak ennyi fér a malac memóriájába). A 
két játékos először leírja a programot (úgy, hogy azt a másik játékos ne lássa), 
majd egyszerre futtatják le őket, azaz a robotok szimultán teszik meg a 
programjuk által előírt 5 lépést. 
A program az alábbi utasításokat tartalmazhatja: 
• előre, hátra, balra, jobbra: egy mezőnyi lépés a megadott irányba, közben a 
robot iránya nem változik. 
• fordulás balra, jobbra: a robot nem vált mezőt, de a megadott irányba 
fordul. 
• tűz: támadás előre a lézerágyúval. 
• ütés: támadás a támadóököllel. 
Amennyiben a robot olyan mezőre akar lépni, ahol a másik robot helyezkedik, 
akkor nem léphet (átugorja az utasítást), amennyiben a két robot ugyanoda akar 
lépni, akkor egyikük se lép (mindkettő átugorja az utasítást). 
A két malac a lézerrel és az ököllel támadhatja egymást. A lézer előre lő, és 
függetlenül a távolságtól eltalálja a másikat. Az ütés pedig valamennyi 
szomszédos mezőn (azaz egy 3 × 3-as négyzetben) eltalálja a másikat. A csatának 
akkor van vége, ha egy robotot háromszor eltaláltak. 
A program biztosítson lehetőséget új játék kezdésére a pályaméret megadásával 
(4 × 4, 6 × 6, 8 × 8), valamint játék mentésére és betöltésére. Ismerje fel, ha vége 
a játéknak, és jelenítse meg, melyik játékos győzött. Játék közben folyamatosan 
jelenítse meg a játékosok aktuális sérülésszámait.
