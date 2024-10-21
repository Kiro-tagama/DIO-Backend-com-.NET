-- 1
select Nome,Ano from Filmes

-- 2
SELECT * FROM Filmes ORDER by ano

-- 3
SELECT * FROM Filmes WHERE nome = 'De Volta Para O Futuro'

-- 4
SELECT * from Filmes WHERE ano = 1997

-- 5
SELECT * from Filmes WHERE ano >= 2000

-- 6
SELECT * FROM filmes WHERE duracao > 100 AND duracao < 150 ORDER BY Duracao

-- 7
SELECT ano, COUNT(1) AS quantidade FROM Filmes
GROUP BY ano
ORDER BY quantidade DESC;

-- 8 
SELECT * from Atores where genero = 'M'

-- 9
SELECT * from Atores where genero = 'F' ORDER by primeironome

-- 10
SELECT f.Nome, g.Genero 
FROM Filmes f
INNER JOIN FilmesGenero fg ON f.Id = fg.IdFilme
INNER JOIN Generos g ON fg.IdGenero = g.Id

-- 11
SELECT f.Nome, g.Genero 
from Filmes f
inner JOIN FilmesGenero fg on f.Id = fg.IdFilme
inner join Generos g on fg.IdGenero = g.Id
WHERE g.Genero = 'Mistério'

-- 12
SELECT f.Nome, a.PrimeiroNome, a.UltimoNome, ef.Papel 
from Filmes f
inner join ElencoFilme ef on f.Id = ef.IdFilme 
inner join Atores a on a.Id = ef.IdAtor

-- nota sql 
/*
  ref ao 7
    adiciona um contador e nomeia de quantidade `count(*) as quantidade`

  ref aos ultimos 10,11,12
    ele atribua iniciais qualquer (pode ser qualquer coisa) f,fg,a,ef,g
    adiciona as referencias (Filme f) e etc
    inner join - junção interna 
    ai cria a assimilação, on id do filmes é igual o id do elenco (n importa a ordem)

*/