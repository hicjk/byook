﻿// <auto-generated />
using System;
using Byook.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Byook.DataAccess.Migrations
{
    [DbContext(typeof(ByookDbContext))]
    [Migration("20220705092322_ModifyProduct")]
    partial class ModifyProduct
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.6");

            modelBuilder.Entity("Byook.Models.Consumer", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(15)
                        .HasColumnType("TEXT")
                        .HasComment("아이디");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT")
                        .HasComment("구매자 주소");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("TEXT")
                        .HasComment("성명");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("TEXT")
                        .HasComment("비밀번호");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("TEXT")
                        .HasComment("핸드폰 번호");

                    b.HasKey("Id");

                    b.ToTable("Consumer");
                });

            modelBuilder.Entity("Byook.Models.Order", b =>
                {
                    b.Property<string>("OrderId")
                        .HasMaxLength(18)
                        .HasColumnType("TEXT")
                        .HasComment("주문번호");

                    b.Property<string>("ConsumerId")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("TEXT")
                        .HasComment("소비자");

                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER")
                        .HasComment("상품번호");

                    b.HasKey("OrderId");

                    b.HasIndex("ConsumerId");

                    b.HasIndex("ProductId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("Byook.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasComment("상품번호");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasComment("제품명");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("TEXT")
                        .HasComment("등록날짜");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Price")
                        .HasColumnType("INTEGER")
                        .HasComment("가격");

                    b.Property<string>("ProductId")
                        .HasColumnType("TEXT");

                    b.Property<string>("SellerId")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("TEXT")
                        .HasComment("사업자등록번호");

                    b.Property<int>("Weight")
                        .HasColumnType("INTEGER")
                        .HasComment("무게");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("Byook.Models.Seller", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(11)
                        .HasColumnType("TEXT")
                        .HasComment("사업자등록번호");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT")
                        .HasComment("판매자 주소");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("TEXT")
                        .HasComment("대표자명");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("TEXT")
                        .HasComment("비밀번호");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("TEXT")
                        .HasComment("핸드폰번호");

                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TradeName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT")
                        .HasComment("상호명");

                    b.HasKey("Id");

                    b.ToTable("Seller");
                });

            modelBuilder.Entity("Byook.Models.Order", b =>
                {
                    b.HasOne("Byook.Models.Consumer", "Consumer")
                        .WithMany()
                        .HasForeignKey("ConsumerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Byook.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Consumer");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Byook.Models.Product", b =>
                {
                    b.HasOne("Byook.Models.Seller", null)
                        .WithMany("Products")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("Byook.Models.Seller", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
