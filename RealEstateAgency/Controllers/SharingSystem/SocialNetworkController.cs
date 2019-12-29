using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using RealEstateAgency.Implementations.ApiImplementations.Services.Contracts;
using RealEstateAgency.Controllers.Contracts;
using RealEstateAgency.DAL.Models;
using RealEstateAgency.Dtos.ModelDtos.SharingSystem;
using RealEstateAgency.Implementations.Authentication;
using RealEstateAgency.Implementations.Providers;

namespace RealEstateAgency.Controllers.SharingSystem
{
    public class SharePropertyDto
    {
        [Required]
        public int PropertyId { get; set; }
        public string SocialNetworkCode { get; set; }
        public string LanguageCode { get; set; }
    }

    public class SocialNetworkController : ModelController<SocialNetwork, SocialNetworkDto>
    {

        public SocialNetworkController(IModelService<SocialNetwork, SocialNetworkDto> modelService) : base(modelService)
        {
        }

        public override Func<IQueryable<SocialNetwork>, IQueryable<SocialNetworkDto>> DtoConverter =>
            items => items.Select(i => new SocialNetworkDto
            {
                Id = i.Id,
                Name = i.Name,
                UniqueKey = i.UniqueKey,
                LogoPicture = i.LogoPicture
            });

    }
}
