// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!2");

Pessoa pessoa = new Pessoa();

pessoa.Name = "John Doe";
pessoa.Age = 30;

pessoa.IntroduceYourSelf();

string texto = "123e";
int numero;
Console.WriteLine(int.TryParse(texto, out numero) +"=="+ numero);