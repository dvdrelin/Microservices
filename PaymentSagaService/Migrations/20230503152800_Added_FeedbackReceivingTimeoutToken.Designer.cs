﻿using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PaymentSagaService.Database;

namespace PaymentSagaService.Migrations;

[DbContext(typeof(StateMachinesDbContext))]
[Migration("20211108065608_Added_FeedbackReceivingTimeoutToken")]
partial class Added_FeedbackReceivingTimeoutToken
{
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("Relational:MaxIdentifierLength", 63)
            .HasAnnotation("ProductVersion", "5.0.11")
            .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

        modelBuilder.Entity("OrderOrchecstratorService.Database.Models.CartPosition", b =>
        {
            b.Property<Guid>("Id")
                .HasColumnType("uuid");

            b.Property<int>("Amount")
                .HasColumnType("integer");

            b.Property<string>("Name")
                .IsRequired()
                .HasColumnType("text");

            b.Property<Guid>("OrderId")
                .HasColumnType("uuid");

            b.Property<int>("Price")
                .HasColumnType("integer");

            b.HasKey("Id");

            b.HasIndex("Id")
                .IsUnique();

            b.HasIndex("OrderId");

            b.ToTable("CartPositions");
        });

        modelBuilder.Entity("OrderOrchecstratorService.StateMachines.OrderStateMachine.OrderState", b =>
        {
            b.Property<Guid>("CorrelationId")
                .HasColumnType("uuid");

            b.Property<DateTimeOffset?>("ConfirmationDate")
                .HasColumnType("timestamp with time zone");

            b.Property<int>("CurrentState")
                .HasColumnType("integer");

            b.Property<DateTimeOffset?>("DeliveryDate")
                .HasColumnType("timestamp with time zone");

            b.Property<Guid?>("FeedbackReceivingTimeoutToken")
                .HasColumnType("uuid");

            b.Property<bool>("IsConfirmed")
                .HasColumnType("boolean");

            b.Property<string>("Manager")
                .HasColumnType("text");

            b.Property<byte[]>("RowVersion")
                .IsConcurrencyToken()
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("bytea");

            b.Property<DateTimeOffset?>("SubmitDate")
                .HasColumnType("timestamp with time zone");

            b.Property<int>("TotalPrice")
                .HasColumnType("integer");

            b.HasKey("CorrelationId");

            b.HasIndex("CorrelationId")
                .IsUnique();

            b.ToTable("OrderStates");
        });

        modelBuilder.Entity("OrderOrchecstratorService.Database.Models.CartPosition", b =>
        {
            b.HasOne("OrderOrchecstratorService.StateMachines.OrderStateMachine.OrderState", "OrderState")
                .WithMany("Cart")
                .HasForeignKey("OrderId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("OrderState");
        });

        modelBuilder.Entity("OrderOrchecstratorService.StateMachines.OrderStateMachine.OrderState", b =>
        {
            b.Navigation("Cart");
        });
#pragma warning restore 612, 618
    }
}