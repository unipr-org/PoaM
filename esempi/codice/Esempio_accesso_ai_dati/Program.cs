
using AccessoAiDati;
using Microsoft.EntityFrameworkCore;
using System.Data;



var contextOptions = new DbContextOptionsBuilder<ConcorrenzaDbContext>()
    .UseSqlServer("Server=localhost,2433;Database=UNIPR;User Id=sa;Password=p4ssw0rD;Encrypt=False")
    .Options;

using (var context0 = new ConcorrenzaDbContext(contextOptions))
{
    var p = new Prodotti()
    {
        CodiceProdotto = "P001",
        Quantita = 100
    };

    var pAlt = new ProdottiAlt()
    {
        CodiceProdotto = "P001",
        Quantita = 100,
        Version = Guid.NewGuid().ToString()
    };

    context0.Prodotti.Add(p);
    context0.ProdottiAlt.Add(pAlt);
    context0.SaveChanges();
}


using (var context1 = new ConcorrenzaDbContext(contextOptions))
using (var context2 = new ConcorrenzaDbContext(contextOptions))
{
    var prodotto1 = context1.Prodotti.Where(p => p.CodiceProdotto == "P001").Single();
    var prodotto2 = context2.Prodotti.Where(p => p.CodiceProdotto == "P001").Single();

    prodotto1.Quantita += 1;
    prodotto2.Quantita += 1;

    context1.SaveChanges();
    context2.SaveChanges(); // lancia eccezione DbUpdateConcurrencyException
}



using (var context1 = new ConcorrenzaDbContext(contextOptions))
using (var context2 = new ConcorrenzaDbContext(contextOptions))
{
    var prodotto1 = context1.ProdottiAlt.Where(p => p.CodiceProdotto == "P001").Single();
    var prodotto2 = context2.ProdottiAlt.Where(p => p.CodiceProdotto == "P001").Single();

    prodotto1.Quantita += 1;
    prodotto2.Quantita += 1;

    prodotto1.Version = Guid.NewGuid().ToString();
    prodotto2.Version = Guid.NewGuid().ToString();

    context1.SaveChanges();
    context2.SaveChanges(); // lancia eccezione DbUpdateConcurrencyException
}




