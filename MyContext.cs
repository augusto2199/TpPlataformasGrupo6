using Microsoft.EntityFrameworkCore;
using InterfazTP.Data;

namespace InterfazTP
{
    internal class MyContext: DbContext
    {
        public DbSet<Usuario> usuarios { get; set; }

        public DbSet<CajaDeAhorro> cajaAhorro { get; set; }
        public DbSet<TarjetaDeCredito> tarjetaCredito { get; set; }
        public DbSet<Movimiento> movimiento { get; set; }
        public DbSet<Pago> pago { get; set; }
        public DbSet<PlazoFijo> plazoFijo { get; set; }

        public MyContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Properties.Resources.connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Nombres de las tablas
            modelBuilder.Entity<Usuario>()
                .ToTable("Usuarios")
                .HasKey(u => u.id_usuario);

            modelBuilder.Entity<CajaDeAhorro>()
                .ToTable("Caja_de_Ahorro")
                .HasKey(c => c.id);

            modelBuilder.Entity<TarjetaDeCredito>()
                .ToTable("Tarjeta_de_Credito")
                .HasKey(t => t.id);

            modelBuilder.Entity<Movimiento>()
                .ToTable("Movimientos")
                .HasKey(m => m.id_movimiento);

            modelBuilder.Entity<Pago>()
                .ToTable("Pagos")
                .HasKey(p => p.id);

            modelBuilder.Entity<PlazoFijo>()
                .ToTable("Plazo_Fijo")
                .HasKey(pf => pf.id);

            // Propiedades de los datos
            // Usuario
            modelBuilder.Entity<Usuario>(
                user =>
                {
                    user.Property(u => u.dni).HasColumnType("int").IsRequired(true);
                    user.Property(u => u.nombre).HasColumnType("varchar(50)").IsRequired(true);
                    user.Property(u => u.apellido).HasColumnType("varchar(50)").IsRequired(true);
                    user.Property(u => u.email).HasColumnType("varchar(512)").IsRequired(true);
                    user.Property(u => u.usuario).HasColumnType("varchar(50)").IsRequired(true);
                    user.Property(u => u.password).HasColumnType("varchar(50)").IsRequired(true);
                });

            // Caja de Ahorro
            modelBuilder.Entity<CajaDeAhorro>(
                caja =>
                {
                    caja.Property(c => c.cbu).HasColumnType("int").IsRequired(true);
                    caja.Property(c => c.saldo).HasColumnType("real").IsRequired(true);
                });

            // Tarjeta de Credito
            modelBuilder.Entity<TarjetaDeCredito>(
                tarjeta =>
                {
                    tarjeta.Property(t => t.numero).HasColumnType("int").IsRequired(true);
                    tarjeta.Property(t => t.codigoV).HasColumnType("int").IsRequired(true);
                    tarjeta.Property(t => t.limite).HasColumnType("real").IsRequired(true);
                    tarjeta.Property(t => t.consumos).HasColumnType("real").IsRequired(true);
                });

            // Plazo Fijo
            modelBuilder.Entity<PlazoFijo>(
                plazoFijo =>
                {
                    plazoFijo.Property(pf => pf.monto).HasColumnType("real").IsRequired(true);
                    plazoFijo.Property(pf => pf.fechaIni).HasColumnType("date");
                    plazoFijo.Property(pf => pf.fechaFin).HasColumnType("date");
                    plazoFijo.Property(pf => pf.tasa).HasColumnType("real");
                });

            // Pago
            modelBuilder.Entity<Pago>(
                pago =>
                {
                    pago.Property(p => p.nombre).HasColumnType("varchar(50)");
                    pago.Property(p => p.monto).HasColumnType("real").IsRequired(true);
                    pago.Property(p => p.metodo).HasColumnType("varchar(50)");
                });

            // Movimiento
            modelBuilder.Entity<Movimiento>(
                movimiento =>
                {
                    movimiento.Property(m => m.detalle).HasColumnType("varchar(50)");
                    movimiento.Property(m => m.monto).HasColumnType("real");
                    movimiento.Property(m => m.fecha).HasColumnType("date");
                });

            // Relacion Usuario - Caja
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.cajas)
                .WithMany(c => c.Titulares)
                .UsingEntity<UsuarioCajaDeAhorro>(
                    //
                    euc => euc.HasOne(up => up.cajaAhorro).WithMany(c => c.UsuarioCaja).HasForeignKey(c => c.fk_cajaAhorro),
                    euc => euc.HasOne(up => up.usuario).WithMany(c => c.UsuarioCaja).HasForeignKey(c => c.fk_usuario),
                    euc => euc.HasKey(k => k.id_UsuarioCaja)
                    );

            // Relacion Usuario – Tarjetas
            modelBuilder.Entity<TarjetaDeCredito>()
                .HasOne(t => t.titular)
                .WithMany(u => u.tarjetas)
                .HasForeignKey(t => t.num_usr)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacion Usuario – Plazo Fijo
            modelBuilder.Entity<PlazoFijo>()
                .HasOne(p => p.titular)
                .WithMany(u => u.pf)
                .HasForeignKey(p => p.num_usr)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacion Usuario - Pago
            modelBuilder.Entity<Pago>()
                .HasOne(p => p.usuario)
                .WithMany(u => u.pagos)
                .HasForeignKey(p => p.num_usr)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacion Caja de Ahorro - Movimientos
            modelBuilder.Entity<Movimiento>()
                .HasOne(m => m.caja)
                .WithMany(c => c.movimientos)
                .HasForeignKey(m => m.num_caja)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Ignore<Banco>();

        }
    }
}

