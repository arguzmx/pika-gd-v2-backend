using CouchDB.Driver;
using CouchDB.Driver.Options;
using pika.modelo.contenido;

namespace pika.servicios.contenido.dbcontext;

//public class ContenidoCouchDbContext : CouchContext
//{
//    public ContenidoCouchDbContext(CouchOptions<ContenidoCouchDbContext> options) : base(options)
//    {

//    }

//    protected override void OnConfiguring(CouchOptionsBuilder optionsBuilder)
//    {

//    }

//    public CouchDatabase<Contenido> Contenidos { get; set; }

//    protected override void OnDatabaseCreating(CouchDatabaseBuilder databaseBuilder)
//    {
//        databaseBuilder.Document<Contenido>().ToDatabase("contenidos");
//    }
//}
