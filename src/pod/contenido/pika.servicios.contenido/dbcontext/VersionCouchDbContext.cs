using CouchDB.Driver;
using CouchDB.Driver.Options;
using pika.modelo.contenido;

namespace pika.servicios.contenido.dbcontext;

public class VersionCouchDbContext : CouchContext
{
    public const string CollecionVolumenes = "cont$volumenes";
    public VersionCouchDbContext(CouchOptions<VersionCouchDbContext> options) : base(options)
    {

    }

    protected override void OnConfiguring(CouchOptionsBuilder optionsBuilder)
    {

    }

    public CouchDatabase<EntidadVersion> Versiones { get; set; }

    protected override void OnDatabaseCreating(CouchDatabaseBuilder databaseBuilder)
    {
        databaseBuilder.Document<EntidadVersion>().ToDatabase(CollecionVolumenes);
    }
}
