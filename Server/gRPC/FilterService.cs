using FeatureDBPortal.Server.Data.Models;
using FeatureDBPortal.Server.Repositories;
using Grpc.Core;
using GrpcCombination;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.gRPC
{
    public class FilterService : Filter.FilterBase
    {
        private readonly IGenericRepository<Application> _applicationRepository;
        private readonly IGenericRepository<Probe> _probeRepository;
        private readonly IGenericRepository<LogicalModel> _modelRepository;
        private readonly IGenericRepository<MinorVersionAssociation> _versionRepository;
        private readonly IGenericRepository<Country> _countryRepository;
        private readonly IGenericRepository<BiopsyKits> _kitRepository;
        private readonly IGenericRepository<Option> _optionRepository;

        public FilterService(
            IGenericRepository<Application> applicationRepository,
            IGenericRepository<Probe> probeRepository,
            IGenericRepository<LogicalModel> modelRepository,
            IGenericRepository<MinorVersionAssociation> versionRepository,
            IGenericRepository<Country> countryRepository,
            IGenericRepository<BiopsyKits> kitRepository,
            IGenericRepository<Option> optionRepository)
        {
            _applicationRepository = applicationRepository;
            _probeRepository = probeRepository;
            _modelRepository = modelRepository;
            _versionRepository = versionRepository;
            _countryRepository = countryRepository;
            _kitRepository = kitRepository;
            _optionRepository = optionRepository;
        }

        async public override Task<ApplicationReplay> GetApplications(EmptyParam request, ServerCallContext context)
        {
            var result = _applicationRepository.Get();

            var reply = new ApplicationReplay();
            reply.Applications
                .AddRange(result
                .Select(item => new ApplicationGRPC { Id = item.Id, Name = item.Name })
                .OrderBy(item => item.Name));

            return await Task.FromResult(reply);
        }

        async public override Task<ProbeReplay> GetProbes(EmptyParam request, ServerCallContext context)
        {
            var result = _probeRepository.Get();

            var reply = new ProbeReplay();
            reply.Probes
                .AddRange(result
                .Select(item => new ProbeGRPC { Id = item.Id, Name = item.Name })
                .OrderBy(item => item.Name));

            return await Task.FromResult(reply);
        }

        async public override Task<ModelReplay> GetModels(EmptyParam request, ServerCallContext context)
        {
            var result = _modelRepository.Get();

            var reply = new ModelReplay();
            reply.Models
                .AddRange(result
                .Select(item => new ModelGRPC { Id = item.Id, Name = item.Name })
                .OrderBy(item => item.Name));

            return await Task.FromResult(reply);
        }

        async public override Task<VersionReplay> GetVersions(EmptyParam request, ServerCallContext context)
        {
            var result = _versionRepository.Get();

            var reply = new VersionReplay();
            reply.Versions
                .AddRange(result
                .Select(item => new VersionGRPC { Id = item.Id, BuildVersion = item.BuildVersion })
                .OrderBy(item => item.BuildVersion));

            return await Task.FromResult(reply);
        }

        async public override Task<CountryReplay> GetCountries(EmptyParam request, ServerCallContext context)
        {
            var result = _countryRepository.Get();

            var reply = new CountryReplay();
            reply.Countries
                .AddRange(result
                .Select(item => new CountryGRPC { Id = item.Id, Name = item.Name })
                .OrderBy(item => item.Name));

            return await Task.FromResult(reply);
        }

        async public override Task<KitReplay> GetKits(EmptyParam request, ServerCallContext context)
        {
            var result = _kitRepository.Get();

            var reply = new KitReplay();
            reply.Kits
                .AddRange(result
                .Select(item => new KitGRPC { Id = item.Id, Name = item.Name })
                .OrderBy(item => item.Name));

            return await Task.FromResult(reply);
        }

        async public override Task<OptionsReplay> GetOptions(EmptyParam request, ServerCallContext context)
        {
            var result = _optionRepository.Get();

            var reply = new OptionsReplay();
            reply.Options
                .AddRange(result
                .Select(item => new OptionGRPC { Id = item.Id, Name = item.Name })
                .OrderBy(item => item.Name));

            return await Task.FromResult(reply);
        }
    }
}