﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RealEstateAgency.DAL.Models
{
    public partial class RealEstateDbContext : DbContext
    {
        public RealEstateDbContext()
        {
        }

        public RealEstateDbContext(DbContextOptions<RealEstateDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ActionType> ActionType { get; set; }
        public virtual DbSet<ActionTypeTranslate> ActionTypeTranslate { get; set; }
        public virtual DbSet<Agent> Agent { get; set; }
        public virtual DbSet<AuthenticationProvider> AuthenticationProvider { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<CityTranslate> CityTranslate { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<CountryTranslate> CountryTranslate { get; set; }
        public virtual DbSet<Currency> Currency { get; set; }
        public virtual DbSet<ImportantPlaceType> ImportantPlaceType { get; set; }
        public virtual DbSet<ImportantPlaceTypeTranslate> ImportantPlaceTypeTranslate { get; set; }
        public virtual DbSet<Language> Language { get; set; }
        public virtual DbSet<MarketingAssistant> MarketingAssistant { get; set; }
        public virtual DbSet<PriceScaleUnit> PriceScaleUnit { get; set; }
        public virtual DbSet<PriceScaleUnitTranslate> PriceScaleUnitTranslate { get; set; }
        public virtual DbSet<Property> Property { get; set; }
        public virtual DbSet<PropertyAdditionalDetail> PropertyAdditionalDetail { get; set; }
        public virtual DbSet<PropertyAroundImportantLandmark> PropertyAroundImportantLandmark { get; set; }
        public virtual DbSet<PropertyAttachment> PropertyAttachment { get; set; }
        public virtual DbSet<PropertyDetail> PropertyDetail { get; set; }
        public virtual DbSet<PropertyFeature> PropertyFeature { get; set; }
        public virtual DbSet<PropertyFeatureTranslate> PropertyFeatureTranslate { get; set; }
        public virtual DbSet<PropertyFloorPlan> PropertyFloorPlan { get; set; }
        public virtual DbSet<PropertyImage> PropertyImage { get; set; }
        public virtual DbSet<PropertyInvolveFeature> PropertyInvolveFeature { get; set; }
        public virtual DbSet<PropertyLabel> PropertyLabel { get; set; }
        public virtual DbSet<PropertyLabelTranslate> PropertyLabelTranslate { get; set; }
        public virtual DbSet<PropertyLocation> PropertyLocation { get; set; }
        public virtual DbSet<PropertyPrice> PropertyPrice { get; set; }
        public virtual DbSet<PropertyStatus> PropertyStatus { get; set; }
        public virtual DbSet<PropertyStatusTranslate> PropertyStatusTranslate { get; set; }
        public virtual DbSet<PropertyType> PropertyType { get; set; }
        public virtual DbSet<PropertyTypeTranslate> PropertyTypeTranslate { get; set; }
        public virtual DbSet<RealEstate> RealEstate { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        public virtual DbSet<RegionTranslate> RegionTranslate { get; set; }
        public virtual DbSet<Request> Request { get; set; }
        public virtual DbSet<RequestAction> RequestAction { get; set; }
        public virtual DbSet<RequestActionFollowUp> RequestActionFollowUp { get; set; }
        public virtual DbSet<RequestAgent> RequestAgent { get; set; }
        public virtual DbSet<RequestProperty> RequestProperty { get; set; }
        public virtual DbSet<RequestState> RequestState { get; set; }
        public virtual DbSet<RequestType> RequestType { get; set; }
        public virtual DbSet<RequestTypeTranslate> RequestTypeTranslate { get; set; }
        public virtual DbSet<SharedProperty> SharedProperty { get; set; }
        public virtual DbSet<SharedPropertyClick> SharedPropertyClick { get; set; }
        public virtual DbSet<SocialNetwork> SocialNetwork { get; set; }
        public virtual DbSet<UserAccount> UserAccount { get; set; }
        public virtual DbSet<UserAccountGroup> UserAccountGroup { get; set; }
        public virtual DbSet<UserAccountWishList> UserAccountWishList { get; set; }
        public virtual DbSet<UserGroup> UserGroup { get; set; }
        public virtual DbSet<UserGroupTranslate> UserGroupTranslate { get; set; }
        public virtual DbSet<Workflow> Workflow { get; set; }
        public virtual DbSet<WorkflowStep> WorkflowStep { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.;Database=RealEstateDb;Trusted_Connection=True;MultipleActiveResultSets=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<ActionType>(entity =>
            {
                entity.ToTable("ActionType", "CRM");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<ActionTypeTranslate>(entity =>
            {
                entity.ToTable("ActionTypeTranslate", "CRM");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.ActionType)
                    .WithMany(p => p.ActionTypeTranslate)
                    .HasForeignKey(d => d.ActionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ActionTypeTranslate_ActionType");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.ActionTypeTranslate)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ActionTypeTranslate_Language");
            });

            modelBuilder.Entity<Agent>(entity =>
            {
                entity.ToTable("Agent", "Organization");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(2048);

                entity.Property(e => e.MetadataJson)
                    .IsRequired()
                    .HasColumnName("Metadata_JSON")
                    .HasMaxLength(2048);

                entity.Property(e => e.UserAccountIdDeleteBy).HasColumnName("UserAccountId_DeleteBy");

                entity.HasOne(d => d.RealEstate)
                    .WithMany(p => p.Agent)
                    .HasForeignKey(d => d.RealEstateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Agent_Realestate");

                entity.HasOne(d => d.UserAccount)
                    .WithMany(p => p.AgentUserAccount)
                    .HasForeignKey(d => d.UserAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Agent_UserAccount");

                entity.HasOne(d => d.UserAccountIdDeleteByNavigation)
                    .WithMany(p => p.AgentUserAccountIdDeleteByNavigation)
                    .HasForeignKey(d => d.UserAccountIdDeleteBy)
                    .HasConstraintName("FK_Agent_UserAccount1__DeleteBy");
            });

            modelBuilder.Entity<AuthenticationProvider>(entity =>
            {
                entity.ToTable("AuthenticationProvider", "RBAC");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("City", "Infrastructure");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.City)
                    .HasForeignKey(d => d.RegionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_City_Region");
            });

            modelBuilder.Entity<CityTranslate>(entity =>
            {
                entity.ToTable("CityTranslate", "Infrastructure");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.CityTranslate)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CityTranslate_City");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.CityTranslate)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CityTranslate_Language");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Country", "Infrastructure");

                entity.Property(e => e.Isocode).HasColumnName("ISOCode");

                entity.Property(e => e.Isolong)
                    .HasColumnName("ISOLong")
                    .HasMaxLength(10);

                entity.Property(e => e.Isoshort)
                    .HasColumnName("ISOShort")
                    .HasMaxLength(10);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.OfficialLongForm).HasMaxLength(256);

                entity.Property(e => e.OfficialShortForm).HasMaxLength(256);

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.Country)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Country_Currency");
            });

            modelBuilder.Entity<CountryTranslate>(entity =>
            {
                entity.ToTable("CountryTranslate", "Infrastructure");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.CountryTranslate)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CountryTranslate_Country");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.CountryTranslate)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CountryTranslate_Language");
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.ToTable("Currency", "Infrastructure");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Symbol)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ImportantPlaceType>(entity =>
            {
                entity.ToTable("ImportantPlaceType", "BasicInformation");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.UserAccountIdDeleteBy).HasColumnName("UserAccountId_DeleteBy");

                entity.HasOne(d => d.UserAccountIdDeleteByNavigation)
                    .WithMany(p => p.ImportantPlaceType)
                    .HasForeignKey(d => d.UserAccountIdDeleteBy)
                    .HasConstraintName("FK_ImportantPlaceType_UserAccount");
            });

            modelBuilder.Entity<ImportantPlaceTypeTranslate>(entity =>
            {
                entity.ToTable("ImportantPlaceTypeTranslate", "BasicInformation");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.ImportantPlaceType)
                    .WithMany(p => p.ImportantPlaceTypeTranslate)
                    .HasForeignKey(d => d.ImportantPlaceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ImportantPlaceTypeTranslate_ImportantPlaceType");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.ImportantPlaceTypeTranslate)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ImportantPlaceTypeTranslate_Language");
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.ToTable("Language", "Infrastructure");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MarketingAssistant>(entity =>
            {
                entity.ToTable("MarketingAssistant", "Organization");

                entity.Property(e => e.Description).HasMaxLength(2048);

                entity.Property(e => e.MetadataJson)
                    .IsRequired()
                    .HasColumnName("Metadata_JSON")
                    .HasMaxLength(2048);

                entity.Property(e => e.TrackingCode)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.RealEstate)
                    .WithMany(p => p.MarketingAssistant)
                    .HasForeignKey(d => d.RealEstateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MarketingAssistant_Realestate");

                entity.HasOne(d => d.UserAccount)
                    .WithMany(p => p.MarketingAssistant)
                    .HasForeignKey(d => d.UserAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MarketingAssistant_UserAccount");
            });

            modelBuilder.Entity<PriceScaleUnit>(entity =>
            {
                entity.ToTable("PriceScaleUnit", "Infrastructure");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PriceScaleUnitTranslate>(entity =>
            {
                entity.ToTable("PriceScaleUnitTranslate", "Infrastructure");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.PriceScaleUnitTranslate)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PriceScaleUnitTranslate_Language");

                entity.HasOne(d => d.PriceScaleUnit)
                    .WithMany(p => p.PriceScaleUnitTranslate)
                    .HasForeignKey(d => d.PriceScaleUnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PriceScaleUnitTranslate_PriceScaleUnit");
            });

            modelBuilder.Entity<Property>(entity =>
            {
                entity.ToTable("Property", "Estate");

                entity.Property(e => e.DateUpdated).HasColumnType("date");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.PropertyUniqId).HasMaxLength(16);

                entity.Property(e => e.PublishingDate).HasColumnType("datetime");

                entity.Property(e => e.ReadyForPublishDate).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.UserAccountIdDeleteBy).HasColumnName("UserAccountId_DeleteBy");

                entity.Property(e => e.UserAccountIdPublished).HasColumnName("UserAccountId_Published");

                entity.Property(e => e.UserAccountIdReadyForPublish).HasColumnName("UserAccountId_ReadyForPublish");

                entity.Property(e => e.VideoUrl).HasMaxLength(1024);

                entity.HasOne(d => d.Agent)
                    .WithMany(p => p.Property)
                    .HasForeignKey(d => d.AgentId)
                    .HasConstraintName("FK_Property_agent_property_agent");

                entity.HasOne(d => d.PropertyLabel)
                    .WithMany(p => p.Property)
                    .HasForeignKey(d => d.PropertyLabelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Property_PropertyLabel");

                entity.HasOne(d => d.PropertyStatus)
                    .WithMany(p => p.Property)
                    .HasForeignKey(d => d.PropertyStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Property_PropertyStatus");

                entity.HasOne(d => d.PropertyType)
                    .WithMany(p => p.Property)
                    .HasForeignKey(d => d.PropertyTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Property_PropertyType");

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.Property)
                    .HasForeignKey(d => d.RequestId)
                    .HasConstraintName("FK_Property_Request");

                entity.HasOne(d => d.UserAccountIdDeleteByNavigation)
                    .WithMany(p => p.PropertyUserAccountIdDeleteByNavigation)
                    .HasForeignKey(d => d.UserAccountIdDeleteBy)
                    .HasConstraintName("FK_Property_UserAccount_01_DeleteBy");

                entity.HasOne(d => d.UserAccountIdPublishedNavigation)
                    .WithMany(p => p.PropertyUserAccountIdPublishedNavigation)
                    .HasForeignKey(d => d.UserAccountIdPublished)
                    .HasConstraintName("FK_Property_UserAccount_03_Published");

                entity.HasOne(d => d.UserAccountIdReadyForPublishNavigation)
                    .WithMany(p => p.PropertyUserAccountIdReadyForPublishNavigation)
                    .HasForeignKey(d => d.UserAccountIdReadyForPublish)
                    .HasConstraintName("FK_Property_UserAccount_02_ReadyForPublish");
            });

            modelBuilder.Entity<PropertyAdditionalDetail>(entity =>
            {
                entity.ToTable("PropertyAdditionalDetail", "Estate");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserAccountIdDeleteBy).HasColumnName("UserAccountId_DeleteBy");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.PropertyAdditionalDetail)
                    .HasForeignKey(d => d.PropertyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropertyAdditionalDetail_Property");

                entity.HasOne(d => d.UserAccountIdDeleteByNavigation)
                    .WithMany(p => p.PropertyAdditionalDetail)
                    .HasForeignKey(d => d.UserAccountIdDeleteBy)
                    .HasConstraintName("FK_PropertyAdditionalDetail_UserAccount");
            });

            modelBuilder.Entity<PropertyAroundImportantLandmark>(entity =>
            {
                entity.ToTable("PropertyAroundImportantLandmark", "Estate");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(256);

                entity.Property(e => e.DistanceKm)
                    .HasColumnName("DistanceKM")
                    .HasColumnType("numeric(5, 2)");

                entity.Property(e => e.DistanceMiles).HasColumnType("numeric(5, 2)");

                entity.Property(e => e.UserAccountIdDeleteBy).HasColumnName("UserAccountId_DeleteBy");

                entity.HasOne(d => d.ImportantPlaceType)
                    .WithMany(p => p.PropertyAroundImportantLandmark)
                    .HasForeignKey(d => d.ImportantPlaceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropertyNearImportantPlace_ImportantPlaceType");

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.PropertyAroundImportantLandmark)
                    .HasForeignKey(d => d.PropertyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropertyNearImportantPlace_Property");

                entity.HasOne(d => d.UserAccountIdDeleteByNavigation)
                    .WithMany(p => p.PropertyAroundImportantLandmark)
                    .HasForeignKey(d => d.UserAccountIdDeleteBy)
                    .HasConstraintName("FK_PropertyAroundImportantLandmark_UserAccount");
            });

            modelBuilder.Entity<PropertyAttachment>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("PropertyAttachment", "Estate");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.FileCaption).HasMaxLength(128);

                entity.Property(e => e.FileContent).IsRequired();

                entity.Property(e => e.FileExtension)
                    .IsRequired()
                    .HasMaxLength(16);

                entity.Property(e => e.UploadDate).HasColumnType("datetime");

                entity.Property(e => e.UserAccountIdDeleteBy).HasColumnName("UserAccountId_DeleteBy");

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.PropertyAttachment)
                    .HasForeignKey(d => d.PropertyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropertyAttachment_Property");

                entity.HasOne(d => d.UserAccountIdDeleteByNavigation)
                    .WithMany(p => p.PropertyAttachment)
                    .HasForeignKey(d => d.UserAccountIdDeleteBy)
                    .HasConstraintName("FK_PropertyAttachment_UserAccount");
            });

            modelBuilder.Entity<PropertyDetail>(entity =>
            {
                entity.ToTable("PropertyDetail", "Estate");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.GaragesSize).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.LandArea).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Size).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.PropertyDetail)
                    .HasForeignKey<PropertyDetail>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropertyDetail_Property");
            });

            modelBuilder.Entity<PropertyFeature>(entity =>
            {
                entity.ToTable("PropertyFeature", "BasicInformation");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.UserAccountIdDeleteBy).HasColumnName("UserAccountId_DeleteBy");

                entity.HasOne(d => d.UserAccountIdDeleteByNavigation)
                    .WithMany(p => p.PropertyFeature)
                    .HasForeignKey(d => d.UserAccountIdDeleteBy)
                    .HasConstraintName("FK_PropertyFeature_UserAccount");
            });

            modelBuilder.Entity<PropertyFeatureTranslate>(entity =>
            {
                entity.ToTable("PropertyFeatureTranslate", "BasicInformation");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.PropertyFeatureTranslate)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropertyFeatureTranslate_Language");

                entity.HasOne(d => d.PropertyFeature)
                    .WithMany(p => p.PropertyFeatureTranslate)
                    .HasForeignKey(d => d.PropertyFeatureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropertyFeatureTranslate_PropertyFeature");
            });

            modelBuilder.Entity<PropertyFloorPlan>(entity =>
            {
                entity.ToTable("PropertyFloorPlan", "Estate");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.FloorName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ImageCaption).HasMaxLength(128);

                entity.Property(e => e.ImageContentFull).HasColumnName("ImageContent_Full");

                entity.Property(e => e.ImageContentTumblr).HasColumnName("ImageContent_Tumblr");

                entity.Property(e => e.ImageExtension).HasMaxLength(16);

                entity.Property(e => e.PlanDescription).HasMaxLength(2048);

                entity.Property(e => e.PricePostfix).HasMaxLength(50);

                entity.Property(e => e.SizePostfix).HasMaxLength(50);

                entity.Property(e => e.UploadDate).HasColumnType("datetime");

                entity.Property(e => e.UserAccountIdDeleteBy).HasColumnName("UserAccountId_DeleteBy");

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.PropertyFloorPlan)
                    .HasForeignKey(d => d.PropertyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropertyFloorPlan_Property");

                entity.HasOne(d => d.UserAccountIdDeleteByNavigation)
                    .WithMany(p => p.PropertyFloorPlan)
                    .HasForeignKey(d => d.UserAccountIdDeleteBy)
                    .HasConstraintName("FK_PropertyFloorPlan_UserAccount");
            });

            modelBuilder.Entity<PropertyImage>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("PropertyImage", "Estate");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.ImageCaption).HasMaxLength(128);

                entity.Property(e => e.ImageContentFull)
                    .IsRequired()
                    .HasColumnName("ImageContent_Full");

                entity.Property(e => e.ImageContentTumblr)
                    .IsRequired()
                    .HasColumnName("ImageContent_Tumblr");

                entity.Property(e => e.ImageExtension)
                    .IsRequired()
                    .HasMaxLength(16);

                entity.Property(e => e.UploadDate).HasColumnType("datetime");

                entity.Property(e => e.UserAccountIdDeleteBy).HasColumnName("UserAccountId_DeleteBy");

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.PropertyImage)
                    .HasForeignKey(d => d.PropertyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropertyImage_Property");

                entity.HasOne(d => d.UserAccountIdDeleteByNavigation)
                    .WithMany(p => p.PropertyImage)
                    .HasForeignKey(d => d.UserAccountIdDeleteBy)
                    .HasConstraintName("FK_PropertyImage_UserAccount");
            });

            modelBuilder.Entity<PropertyInvolveFeature>(entity =>
            {
                entity.ToTable("PropertyInvolveFeature", "Estate");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.UserAccountIdDeleteBy).HasColumnName("UserAccountId_DeleteBy");

                entity.HasOne(d => d.PropertyFeature)
                    .WithMany(p => p.PropertyInvolveFeature)
                    .HasForeignKey(d => d.PropertyFeatureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropertyInvolveFeature_PropertyFeature");

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.PropertyInvolveFeature)
                    .HasForeignKey(d => d.PropertyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropertyInvolveFeature_Property");

                entity.HasOne(d => d.UserAccountIdDeleteByNavigation)
                    .WithMany(p => p.PropertyInvolveFeature)
                    .HasForeignKey(d => d.UserAccountIdDeleteBy)
                    .HasConstraintName("FK_PropertyInvolveFeature_UserAccount");
            });

            modelBuilder.Entity<PropertyLabel>(entity =>
            {
                entity.ToTable("PropertyLabel", "BasicInformation");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserAccountIdDeleteBy).HasColumnName("UserAccountId_DeleteBy");

                entity.HasOne(d => d.UserAccountIdDeleteByNavigation)
                    .WithMany(p => p.PropertyLabel)
                    .HasForeignKey(d => d.UserAccountIdDeleteBy)
                    .HasConstraintName("FK_PropertyLabel_UserAccount");
            });

            modelBuilder.Entity<PropertyLabelTranslate>(entity =>
            {
                entity.ToTable("PropertyLabelTranslate", "BasicInformation");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.PropertyLabelTranslate)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropertyLabelTranslate_Language");

                entity.HasOne(d => d.PropertyLabel)
                    .WithMany(p => p.PropertyLabelTranslate)
                    .HasForeignKey(d => d.PropertyLabelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropertyLabelTranslate_PropertyLabel");
            });

            modelBuilder.Entity<PropertyLocation>(entity =>
            {
                entity.ToTable("PropertyLocation", "Estate");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AddressLine1)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.AddressLine2).HasMaxLength(256);

                entity.Property(e => e.GoogleMapsLatitude).HasColumnType("decimal(12, 9)");

                entity.Property(e => e.GoogleMapsLongitude).HasColumnType("decimal(12, 9)");

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.PropertyLocation)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_PropertyLocation_City");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.PropertyLocation)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_PropertyLocation_Country");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.PropertyLocation)
                    .HasForeignKey<PropertyLocation>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropertyLocation_Property");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.PropertyLocation)
                    .HasForeignKey(d => d.RegionId)
                    .HasConstraintName("FK_PropertyLocation_Region");
            });

            modelBuilder.Entity<PropertyPrice>(entity =>
            {
                entity.ToTable("PropertyPrice", "Estate");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AfterPriceLabel).HasMaxLength(50);

                entity.Property(e => e.BeforePriceLabel).HasMaxLength(50);

                entity.Property(e => e.CalculatedPriceUnit).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 4)");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.PropertyPrice)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropertyPrice_Currency");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.PropertyPrice)
                    .HasForeignKey<PropertyPrice>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropertyPrice_Property");

                entity.HasOne(d => d.PriceScaleUnit)
                    .WithMany(p => p.PropertyPrice)
                    .HasForeignKey(d => d.PriceScaleUnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropertyPrice_PriceScaleUnit");
            });

            modelBuilder.Entity<PropertyStatus>(entity =>
            {
                entity.ToTable("PropertyStatus", "BasicInformation");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserAccountIdDeleteBy).HasColumnName("UserAccountId_DeleteBy");

                entity.HasOne(d => d.UserAccountIdDeleteByNavigation)
                    .WithMany(p => p.PropertyStatus)
                    .HasForeignKey(d => d.UserAccountIdDeleteBy)
                    .HasConstraintName("FK_PropertyStatus_UserAccount");
            });

            modelBuilder.Entity<PropertyStatusTranslate>(entity =>
            {
                entity.ToTable("PropertyStatusTranslate", "BasicInformation");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.PropertyStatusTranslate)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropertyStatusTranslate_Language");

                entity.HasOne(d => d.PropertyStatus)
                    .WithMany(p => p.PropertyStatusTranslate)
                    .HasForeignKey(d => d.PropertyStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropertyStatusTranslate_PropertyStatus");
            });

            modelBuilder.Entity<PropertyType>(entity =>
            {
                entity.ToTable("PropertyType", "BasicInformation");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.UserAccountIdDeleteBy).HasColumnName("UserAccountId_DeleteBy");

                entity.HasOne(d => d.UserAccountIdDeleteByNavigation)
                    .WithMany(p => p.PropertyType)
                    .HasForeignKey(d => d.UserAccountIdDeleteBy)
                    .HasConstraintName("FK_PropertyType_UserAccount");
            });

            modelBuilder.Entity<PropertyTypeTranslate>(entity =>
            {
                entity.ToTable("PropertyTypeTranslate", "BasicInformation");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.PropertyTypeTranslate)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropertyTypeTranslate_Language");

                entity.HasOne(d => d.PropertyType)
                    .WithMany(p => p.PropertyTypeTranslate)
                    .HasForeignKey(d => d.PropertyTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropertyTypeTranslate_PropertyType");
            });

            modelBuilder.Entity<RealEstate>(entity =>
            {
                entity.ToTable("RealEstate", "Organization");

                entity.HasIndex(e => e.Name)
                    .HasName("IX_RealEstate")
                    .IsUnique();

                entity.Property(e => e.Address01).HasMaxLength(100);

                entity.Property(e => e.Address02).HasMaxLength(100);

                entity.Property(e => e.DateFormat)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'YYYY-MM-DD')");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Fax).HasMaxLength(20);

                entity.Property(e => e.LanguageIdDefault).HasColumnName("LanguageId_Default");

                entity.Property(e => e.MetadataJson).HasColumnName("Metadata_JSON");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Phone01).HasMaxLength(20);

                entity.Property(e => e.Phone02).HasMaxLength(20);

                entity.Property(e => e.Phone03).HasMaxLength(20);

                entity.Property(e => e.WebsiteUrl).HasMaxLength(256);

                entity.Property(e => e.ZipCode).HasMaxLength(20);

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.RealEstate)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RealEstate_Currency");

                entity.HasOne(d => d.LanguageIdDefaultNavigation)
                    .WithMany(p => p.RealEstate)
                    .HasForeignKey(d => d.LanguageIdDefault)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RealEstate_Language");
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.ToTable("Region", "Infrastructure");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Region)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Region_Country");
            });

            modelBuilder.Entity<RegionTranslate>(entity =>
            {
                entity.ToTable("RegionTranslate", "Infrastructure");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.RegionTranslate)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RegionTranslate_Language");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.RegionTranslate)
                    .HasForeignKey(d => d.RegionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RegionTranslate_Region");
            });

            modelBuilder.Entity<Request>(entity =>
            {
                entity.ToTable("Request", "CRM");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(2048);

                entity.Property(e => e.MarketingAssistantTrackingCode).HasMaxLength(256);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.TrackingNumber)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.UserAccountIdDeleteBy).HasColumnName("UserAccountId_DeleteBy");

                entity.Property(e => e.UserAccountIdRequester).HasColumnName("UserAccountId_Requester");

                entity.HasOne(d => d.PropertyNavigation)
                    .WithMany(p => p.RequestNavigation)
                    .HasForeignKey(d => d.PropertyId)
                    .HasConstraintName("FK_Request_Property");

                entity.HasOne(d => d.RequestType)
                    .WithMany(p => p.Request)
                    .HasForeignKey(d => d.RequestTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Request_RequestType");

                entity.HasOne(d => d.SharedPropertyClick)
                    .WithMany(p => p.Request)
                    .HasForeignKey(d => d.SharedPropertyClickId)
                    .HasConstraintName("FK_Request_SharedPropertyClick");

                entity.HasOne(d => d.UserAccountIdRequesterNavigation)
                    .WithMany(p => p.Request)
                    .HasForeignKey(d => d.UserAccountIdRequester)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Request_UserAccount");

                entity.HasOne(d => d.Workflow)
                    .WithMany(p => p.Request)
                    .HasForeignKey(d => d.WorkflowId)
                    .HasConstraintName("FK_Request_Workflow");
            });

            modelBuilder.Entity<RequestAction>(entity =>
            {
                entity.ToTable("RequestAction", "CRM");

                entity.Property(e => e.ActionDate).HasColumnType("datetime");

                entity.Property(e => e.ActionIsSuccess)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ActionTime)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(2048);

                entity.Property(e => e.MetaDataJson).HasColumnName("MetaData_Json");

                entity.Property(e => e.RequestActionFollowUpReference).HasColumnName("RequestActionFollowUp_Reference");

                entity.HasOne(d => d.ActionType)
                    .WithMany(p => p.RequestAction)
                    .HasForeignKey(d => d.ActionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequestAction_ActionType");

                entity.HasOne(d => d.Agent)
                    .WithMany(p => p.RequestAction)
                    .HasForeignKey(d => d.AgentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequestAction_Agent");

                entity.HasOne(d => d.RequestActionFollowUpReferenceNavigation)
                    .WithMany(p => p.RequestAction)
                    .HasForeignKey(d => d.RequestActionFollowUpReference)
                    .HasConstraintName("FK_RequestAction_RequestActionFollowUp");

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.RequestAction)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequestAction_Request");
            });

            modelBuilder.Entity<RequestActionFollowUp>(entity =>
            {
                entity.ToTable("RequestActionFollowUp", "CRM");

                entity.Property(e => e.AgentIdFollowUp).HasColumnName("AgentId_FollowUp");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.FollowUpDateTime).HasColumnType("datetime");

                entity.Property(e => e.FollowUpDateTimeSnooze)
                    .HasColumnName("FollowUpDateTime_Snooze")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.ActionType)
                    .WithMany(p => p.RequestActionFollowUp)
                    .HasForeignKey(d => d.ActionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequestActionFollowUp_ActionType");

                entity.HasOne(d => d.AgentIdFollowUpNavigation)
                    .WithMany(p => p.RequestActionFollowUp)
                    .HasForeignKey(d => d.AgentIdFollowUp)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequestActionFollowUp_Agent");

                entity.HasOne(d => d.RequestActionNavigation)
                    .WithMany(p => p.RequestActionFollowUp)
                    .HasForeignKey(d => d.RequestActionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequestActionFollowUp_RequestAction");
            });

            modelBuilder.Entity<RequestAgent>(entity =>
            {
                entity.ToTable("RequestAgent", "CRM");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.FromDate).HasColumnType("date");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ToDate).HasColumnType("date");

                entity.HasOne(d => d.Agent)
                    .WithMany(p => p.RequestAgent)
                    .HasForeignKey(d => d.AgentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequestAgent_Agent");

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.RequestAgent)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequestAgent_Request");
            });

            modelBuilder.Entity<RequestProperty>(entity =>
            {
                entity.ToTable("RequestProperty", "CRM");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(2048);

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.RequestProperty)
                    .HasForeignKey(d => d.PropertyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequestProperty_Property");

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.RequestProperty)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequestProperty_Request");
            });

            modelBuilder.Entity<RequestState>(entity =>
            {
                entity.ToTable("RequestState", "CRM");

                entity.Property(e => e.Description).HasMaxLength(2048);

                entity.Property(e => e.FinishedDate).HasColumnType("datetime");

                entity.Property(e => e.StartStepDate).HasColumnType("datetime");

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.RequestState)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequestState_Request");

                entity.HasOne(d => d.WorkflowStep)
                    .WithMany(p => p.RequestState)
                    .HasForeignKey(d => d.WorkflowStepId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequestState_WorkflowStep");
            });

            modelBuilder.Entity<RequestType>(entity =>
            {
                entity.ToTable("RequestType", "CRM");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<RequestTypeTranslate>(entity =>
            {
                entity.ToTable("RequestTypeTranslate", "CRM");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.RequestTypeTranslate)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequestTypeTranslate_Language");

                entity.HasOne(d => d.RequestType)
                    .WithMany(p => p.RequestTypeTranslate)
                    .HasForeignKey(d => d.RequestTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequestTypeTranslate_RequestType");
            });

            modelBuilder.Entity<SharedProperty>(entity =>
            {
                entity.ToTable("SharedProperty", "SharingSystem");

                entity.Property(e => e.ReferralCodeAndSharingNetworkId)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.SharedProperty)
                    .HasForeignKey(d => d.PropertyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SharedProperty_Property");

                entity.HasOne(d => d.SocialNetwork)
                    .WithMany(p => p.SharedProperty)
                    .HasForeignKey(d => d.SocialNetworkId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SharedProperty_SocialNetwork");

                entity.HasOne(d => d.UserAccount)
                    .WithMany(p => p.SharedProperty)
                    .HasForeignKey(d => d.UserAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SharedProperty_UserAccount");
            });

            modelBuilder.Entity<SharedPropertyClick>(entity =>
            {
                entity.ToTable("SharedPropertyClick", "SharingSystem");

                entity.Property(e => e.MetaData).IsRequired();

                entity.HasOne(d => d.SharedProperty)
                    .WithMany(p => p.SharedPropertyClick)
                    .HasForeignKey(d => d.SharedPropertyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SharedPropertyClick_SharedProperty");
            });

            modelBuilder.Entity<SocialNetwork>(entity =>
            {
                entity.ToTable("SocialNetwork", "SharingSystem");

                entity.HasIndex(e => e.Name)
                    .HasName("IX_SocialNetwork")
                    .IsUnique();

                entity.HasIndex(e => e.UniqueKey)
                    .HasName("IX_SocialNetwork_1")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.UniqueKey)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.ToTable("UserAccount", "RBAC");

                entity.HasIndex(e => e.Email)
                    .HasName("IX_UserAccount_2")
                    .IsUnique();

                entity.HasIndex(e => e.ReferralCode)
                    .HasName("IX_UserAccount_1")
                    .IsUnique();

                entity.HasIndex(e => e.UserName)
                    .HasName("IX_UserAccount")
                    .IsUnique();

                entity.Property(e => e.ActivationKey)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Address01).HasMaxLength(100);

                entity.Property(e => e.Address02).HasMaxLength(100);

                entity.Property(e => e.AuthenticationProviderAccessToken)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Email).HasMaxLength(128);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.MiddleName).HasMaxLength(128);

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordSalt)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Phone01).HasMaxLength(20);

                entity.Property(e => e.Phone02).HasMaxLength(20);

                entity.Property(e => e.ReferralCode)
                    .HasMaxLength(258)
                    .IsUnicode(false);

                entity.Property(e => e.RegistrationDate).HasColumnType("datetime");

                entity.Property(e => e.ResetPasswordKey)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.UserName).HasMaxLength(128);

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.AuthenticationProvider)
                    .WithMany(p => p.UserAccount)
                    .HasForeignKey(d => d.AuthenticationProviderId)
                    .HasConstraintName("FK_UserAccount_AuthenticationProvider");
            });

            modelBuilder.Entity<UserAccountGroup>(entity =>
            {
                entity.ToTable("UserAccountGroup", "RBAC");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.HasOne(d => d.UserAccount)
                    .WithMany(p => p.UserAccountGroup)
                    .HasForeignKey(d => d.UserAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserAccountGroup_UserAccount");

                entity.HasOne(d => d.UserGroup)
                    .WithMany(p => p.UserAccountGroup)
                    .HasForeignKey(d => d.UserGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserAccountGroup_UserGroup");
            });

            modelBuilder.Entity<UserAccountWishList>(entity =>
            {
                entity.ToTable("UserAccountWishList", "RBAC");

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.UserAccountWishList)
                    .HasForeignKey(d => d.PropertyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserAccountWishList_Property");

                entity.HasOne(d => d.UserAccount)
                    .WithMany(p => p.UserAccountWishList)
                    .HasForeignKey(d => d.UserAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserAccountWishList_UserAccount");
            });

            modelBuilder.Entity<UserGroup>(entity =>
            {
                entity.ToTable("UserGroup", "RBAC");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.StaticCode)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<UserGroupTranslate>(entity =>
            {
                entity.ToTable("UserGroupTranslate", "RBAC");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.UserGroupTranslate)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserGroupTranslate_Language");

                entity.HasOne(d => d.UserGroup)
                    .WithMany(p => p.UserGroupTranslate)
                    .HasForeignKey(d => d.UserGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserGroupTranslate_UserGroup");
            });

            modelBuilder.Entity<Workflow>(entity =>
            {
                entity.ToTable("Workflow", "CRM");

                entity.HasIndex(e => e.Name)
                    .HasName("IX_Workflow_1")
                    .IsUnique();

                entity.HasIndex(e => e.RequestTypeId)
                    .HasName("IX_Workflow")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.RequestType)
                    .WithOne(p => p.Workflow)
                    .HasForeignKey<Workflow>(d => d.RequestTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Workflow_RequestType");
            });

            modelBuilder.Entity<WorkflowStep>(entity =>
            {
                entity.ToTable("WorkflowStep", "CRM");

                entity.HasIndex(e => new { e.WorkflowId, e.StepNumber })
                    .HasName("IX_WorkflowStep")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.Workflow)
                    .WithMany(p => p.WorkflowStep)
                    .HasForeignKey(d => d.WorkflowId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WorkflowStep_Workflow");
            });
        }
    }
}
