﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Nhom8.Data;

public partial class BookingHotelContext : DbContext
{
    public BookingHotelContext()
    {
    }

    public BookingHotelContext(DbContextOptions<BookingHotelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChiTietPhong> ChiTietPhongs { get; set; }

    public virtual DbSet<Conversation> Conversations { get; set; }

    public virtual DbSet<DatPhong> DatPhongs { get; set; }

    public virtual DbSet<DichVu> DichVus { get; set; }

    public virtual DbSet<KhachSan> KhachSans { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Phong> Phongs { get; set; }

    public virtual DbSet<QuyDinhChung> QuyDinhChungs { get; set; }

    public virtual DbSet<TienNghi> TienNghis { get; set; }

    public virtual DbSet<User> Users { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietPhong>(entity =>
        {
            entity.HasKey(e => e.IdChiTietPhong).HasName("PK__ChiTietP__899DC2B3AD63CE6E");

            entity.ToTable("ChiTietPhong");

            entity.Property(e => e.IdChiTietPhong).HasColumnName("ID_ChiTietPhong");
            entity.Property(e => e.IdPhong).HasColumnName("ID_Phong");
            entity.Property(e => e.SlPhong).HasColumnName("Sl_Phong");

            entity.HasOne(d => d.IdPhongNavigation).WithMany(p => p.ChiTietPhongs)
                .HasForeignKey(d => d.IdPhong)
                .HasConstraintName("FK_ChiTietPhong_Phong");
        });

        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.HasKey(e => e.ConversationId).HasName("PK__Conversa__C050D8773E66B26F");

            entity.ToTable("Conversation");

            entity.Property(e => e.StartTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Conversations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Conversation_Users");
        });

        modelBuilder.Entity<DatPhong>(entity =>
        {
            entity.HasKey(e => e.IdDatPhong).HasName("PK__DatPhong__E5702892684C4EA2");

            entity.ToTable("DatPhong");

            entity.Property(e => e.IdDatPhong).HasColumnName("ID_DatPhong");
            entity.Property(e => e.IdPhong).HasColumnName("ID_Phong");
            entity.Property(e => e.NgayCheckin).HasColumnName("Ngay_checkin");
            entity.Property(e => e.NgayCheckout).HasColumnName("Ngay_checkout");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.IdPhongNavigation).WithMany(p => p.DatPhongs)
                .HasForeignKey(d => d.IdPhong)
                .HasConstraintName("FK_Phong_DatPhong");

            entity.HasOne(d => d.User).WithMany(p => p.DatPhongs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_DatPhong_Users");
        });

        modelBuilder.Entity<DichVu>(entity =>
        {
            entity.HasKey(e => e.IdDichVu).HasName("PK__DichVu__6C465C9F68FEDDC4");

            entity.ToTable("DichVu");

            entity.Property(e => e.IdDichVu).HasColumnName("ID_DichVu");
            entity.Property(e => e.IdKs).HasColumnName("idKS");
            entity.Property(e => e.TenDichVu).HasMaxLength(100);

            entity.HasOne(d => d.IdKsNavigation).WithMany(p => p.DichVus)
                .HasForeignKey(d => d.IdKs)
                .HasConstraintName("FK_DichVu_KhachSan");
        });

        modelBuilder.Entity<KhachSan>(entity =>
        {
            entity.HasKey(e => e.IdKs).HasName("PK__KhachSan__8B62EC92B5A7BC83");

            entity.ToTable("KhachSan");

            entity.Property(e => e.IdKs).HasColumnName("ID_KS");
            entity.Property(e => e.DanhGia).HasColumnType("text");
            entity.Property(e => e.DiaChi).HasMaxLength(100);
            entity.Property(e => e.ImageKs)
                .HasMaxLength(500)
                .HasColumnName("Image_KS");
            entity.Property(e => e.TenKs)
                .HasMaxLength(100)
                .HasColumnName("TenKS");
            entity.Property(e => e.Tinh).HasMaxLength(100);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.KhachSans)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Users_KhachSan");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__Messages__C87C0C9C71A66986");

            entity.Property(e => e.MessageContent).HasColumnType("text");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Conversation).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ConversationId)
                .HasConstraintName("FK_Messages_Conversation");

            entity.HasOne(d => d.User).WithMany(p => p.Messages)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Messages_Users");
        });

        modelBuilder.Entity<Phong>(entity =>
        {
            entity.HasKey(e => e.IdPhong).HasName("PK__Phong__DA88924DA2EA232C");

            entity.ToTable("Phong");

            entity.Property(e => e.IdPhong).HasColumnName("ID_Phong");
            entity.Property(e => e.IdKs).HasColumnName("ID_KS");
            entity.Property(e => e.LoaiPhong).HasMaxLength(50);
            entity.Property(e => e.TenPhong).HasMaxLength(50);
            entity.Property(e => e.TinhTrangPhong).HasMaxLength(50);

            entity.HasOne(d => d.IdKsNavigation).WithMany(p => p.Phongs)
                .HasForeignKey(d => d.IdKs)
                .HasConstraintName("FK_Phong_KhachSan");
        });

        modelBuilder.Entity<QuyDinhChung>(entity =>
        {
            entity.HasKey(e => e.IdQuyDinh).HasName("PK__QuyDinhC__C2AF97D6D41ADE7D");

            entity.ToTable("QuyDinhChung");

            entity.Property(e => e.IdQuyDinh).HasColumnName("ID_QuyDinh");
            entity.Property(e => e.IdKs).HasColumnName("idKS");
            entity.Property(e => e.TenQuyDinh).HasMaxLength(100);

            entity.HasOne(d => d.IdKsNavigation).WithMany(p => p.QuyDinhChungs)
                .HasForeignKey(d => d.IdKs)
                .HasConstraintName("FK_QuyDinhChung_KhachSan");
        });

        modelBuilder.Entity<TienNghi>(entity =>
        {
            entity.HasKey(e => e.MaTienNghi).HasName("PK__TienNghi__ED7B8F4DB7CEE604");

            entity.ToTable("TienNghi");

            entity.Property(e => e.IdChiTietPhong).HasColumnName("ID_ChiTietPhong");
            entity.Property(e => e.TenTienNghi).HasMaxLength(100);

            entity.HasOne(d => d.IdChiTietPhongNavigation).WithMany(p => p.TienNghis)
                .HasForeignKey(d => d.IdChiTietPhong)
                .HasConstraintName("FK_TienNghi_ChiTietPhong");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACB5A1E56B");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Img)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IMG");
            entity.Property(e => e.Mk)
                .HasMaxLength(50)
                .HasColumnName("MK");
            entity.Property(e => e.Otp).HasColumnName("OTP");
            entity.Property(e => e.RandomKey)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.Sdt)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("SDT");
            entity.Property(e => e.TenKh)
                .HasMaxLength(50)
                .HasColumnName("TenKH");
            entity.Property(e => e.Tk)
                .HasMaxLength(50)
                .HasColumnName("TK");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}