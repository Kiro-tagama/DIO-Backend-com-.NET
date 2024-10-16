public class Pessoa{
    // em outras linguagens desconheço o uso desse {get,set} o que é um facilitador ao invez de criar um trecho grandes e desnecessario
    public string Name { get; set; }
    public int Age { get; set; }

    public void IntroduceYourSelf(){
       Console.WriteLine($"Olá, me chamo {Name} e tenho {Age} anos.");
    }
}