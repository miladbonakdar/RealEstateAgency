using System;
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
using RealEstateAgency.Shared.Exceptions;

namespace RealEstateAgency.Controllers.SharingSystem
{
    public class SharedPropertyController : ModelController<SharedProperty, SharedPropertyDto>
    {
        private readonly IUserProvider _userProvider;
        private readonly ISocialNetworkProvider _socialNetworkProvider;

        public SharedPropertyController(IModelService<SharedProperty, SharedPropertyDto> modelService,
            IUserProvider userProvider, ISocialNetworkProvider socialNetworkProvider) : base(modelService)
        {
            _userProvider = userProvider;
            _socialNetworkProvider = socialNetworkProvider;
        }

        public override Func<IQueryable<SharedProperty>, IQueryable<SharedPropertyDto>> DtoConverter =>
            items => items.Select(i => new SharedPropertyDto
            {
                Id = i.Id,
                PropertyId = i.PropertyId,
                UserAccountId = i.UserAccountId,
                SocialNetworkId = i.SocialNetworkId,
                ReferralCodeAndSharingNetworkId = i.ReferralCodeAndSharingNetworkId
            });

        [HttpPost("[Action]")]
        public async Task<ActionResult<string>> ShareProperty(SharePropertyDto dto, CancellationChangeToken token)
        {
            var sn = _socialNetworkProvider[dto.SocialNetworkCode];
            if(sn is null) throw  new AppException("Social media is not valid");
            var shareItem = await ModelService.GetAsync(i => i.PropertyId == dto.PropertyId &&
                                                       i.SocialNetworkId == sn.Id &&
                                                       i.UserAccountId == _userProvider.Id);

        }
    }
}
