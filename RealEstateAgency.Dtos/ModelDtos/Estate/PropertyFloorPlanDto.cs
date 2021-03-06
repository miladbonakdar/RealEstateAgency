﻿using RealEstateAgency.DAL.DtoContracts;
using RealEstateAgency.DAL.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace RealEstateAgency.Dtos.ModelDtos.Estate
{
    public class PropertyFloorPlanDto : ModelDtoBase<PropertyFloorPlan>
    {
        public override int Id { get; set; }

        [Required]
        public int PropertyId { get; set; }

        [Required]
        public string FloorName { get; set; }

        public int? FloorPrice { get; set; }
        public string PricePostfix { get; set; }
        public int? FloorSize { get; set; }
        public string SizePostfix { get; set; }
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public string PlanDescription { get; set; }
        public string ImageCaption { get; set; }
        public DateTime? UploadDate { get; set; }
        public string ImageExtension { get; set; }
        public int? ImageSize { get; set; }
        public string ImageContentTumblrUrl { get; set; }
        public string ImageContentFullUrl { get; set; }
        public byte[] ImageContentTumblr { get; set; }

        public override IModelDto<PropertyFloorPlan> From(PropertyFloorPlan entity)
        {
            Id = entity.Id;
            PropertyId = entity.PropertyId;
            FloorName = entity.FloorName;
            FloorPrice = entity.FloorPrice;
            PricePostfix = entity.PricePostfix;
            FloorSize = entity.FloorSize;
            SizePostfix = entity.SizePostfix;
            Bedrooms = entity.Bedrooms;
            Bathrooms = entity.Bathrooms;
            PlanDescription = entity.PlanDescription;
            ImageCaption = entity.ImageCaption;
            UploadDate = entity.UploadDate;
            ImageExtension = entity.ImageExtension;
            ImageSize = entity.ImageSize;
            return this;
        }

        public override PropertyFloorPlan Create() =>
            new PropertyFloorPlan
            {
                PropertyId = PropertyId,
                FloorName = FloorName,
                FloorPrice = FloorPrice,
                PricePostfix = PricePostfix,
                FloorSize = FloorSize,
                SizePostfix = SizePostfix,
                Bedrooms = Bedrooms,
                Bathrooms = Bathrooms,
                PlanDescription = PlanDescription,
                ImageCaption = ImageCaption,
                UploadDate = UploadDate,
                ImageExtension = ImageExtension,
                ImageSize = ImageSize,
            };

        public override PropertyFloorPlan Update() =>
            new PropertyFloorPlan
            {
                Id = Id,
                PropertyId = PropertyId,
                FloorName = FloorName,
                FloorPrice = FloorPrice,
                PricePostfix = PricePostfix,
                FloorSize = FloorSize,
                SizePostfix = SizePostfix,
                Bedrooms = Bedrooms,
                Bathrooms = Bathrooms,
                PlanDescription = PlanDescription,
                ImageCaption = ImageCaption,
                UploadDate = UploadDate,
                ImageExtension = ImageExtension,
                ImageSize = ImageSize,
            };
    }
}
