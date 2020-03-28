Использование FTS в PostgreSQL
===
**1. Тип tsvector**</br>
Задание 1</br>
После выполнения запроса</br>
```sql
SELECT to_tsvector('Съешь ещё этих мягких французских булок, да выпей чаю');
```
был получен результат</br>
```sql
                    to_tsvector
'булок':6 'вып':8 'ещ':2 'мягк':4 'съеш':1 'французск':5 'ча':9 'эт':3
```
В векторе отсутствует слово **да**, т.к. это слово распознаётся словарём как стоп-слово.</br></br>
**2. Тип tsquery**</br>
Задание 2</br>
@@ - оператор типа boolean, проверяющий соответствие tsvector и tsquery.</br>
Из трёх запросов</br>
```sql
--№1
SELECT to_tsvector('The quick brown fox jumped over the lazy dog')  
    @@ to_tsquery('fox');
--№2
SELECT to_tsvector('The quick brown fox jumped over the lazy dog')  
    @@ to_tsquery('foxes');
--№3 
SELECT to_tsvector('The quick brown fox jumped over the lazy dog')  
    @@ to_tsquery('foxhound');
```
первый и второй возвращают **true**, а третий **false**. Это связано с тем, что **fox** и **foxhound** 
являются разными по смыслу словами (fox - лиса, foxhound - порода собаки). Лексема в словаре для слова fox - fox, для foxhound - foxhound.
</br>
В результате запроса
```sql
SELECT to_tsvector('Russian', 'Съешь ещё этих мягких французских булок, да выпей чаю.')
    @@ to_tsquery('Russian','булка');
```
не находится слово `булка`, т.к. лексемы для слов `булка` и `булок` отличаются. Для слова булка - булк, а для булок - булок.
</br>
В результате выполнения двух запросов</br>
```sql
--№1
SELECT to_tsvector('Russian', 'Съешь ещё этих мягких французских пирожков, да выпей чаю.')
    @@ to_tsquery('Russian','пирожки');
--№2
SELECT to_tsvector('Russian', 'Съешь ещё этих мягких французских пирожков, да выпей чаю.')
    @@ to_tsquery('Russian','пирожок');
```
первый выводит true, второй false. Лексема слова `пирожков` - пирожк. Только в первом запросе лексема слова в tsquery будет 
соответсвовать лексеме слова в tsvector (пирожки - пирожк, пирожок - пирожок).
</br></br>
**3. Операторы**</br>
Задание 3</br>