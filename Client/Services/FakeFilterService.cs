using FeatureDBPortal.Shared;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Services
{
    public class FakeFilterService : IFilterService
    {
        const int _count = 10;

        async public Task<IEnumerable<ApplicationDTO>> GetApplicationsAsync()
        {
            var result = new List<ApplicationDTO>();

            for (int i = 0; i < _count; i++)
                result.Add(new ApplicationDTO { Id = i, Name = $"{nameof(ApplicationDTO)} - {i}" });

            return await Task.FromResult(result);
        }

        async public Task<IEnumerable<ProbeDTO>> GetProbesAsync()
        {
            var result = new List<ProbeDTO>();

            for (int i = 0; i < _count; i++)
                result.Add(new ProbeDTO { Id = i, Name = $"{nameof(ProbeDTO)} - {i}" });

            return await Task.FromResult(result);
        }

        async public Task<IEnumerable<CountryDTO>> GetCountriesAsync()
        {
            var result = new List<CountryDTO>();

            for (int i = 0; i < _count; i++)
                result.Add(new CountryDTO { Id = i, CountryName = $"{nameof(CountryDTO)} - {i}" });

            return await Task.FromResult(result);
        }

        async public Task<IEnumerable<VersionDTO>> GetVersionsAsync()
        {
            var result = new List<VersionDTO>();

            for (int i = 0; i < _count; i++)
                result.Add(new VersionDTO { Id = i, BuildVersion = $"{nameof(VersionDTO)} - {i}" });

            return await Task.FromResult(result);
        }

        async public Task<IEnumerable<ModelDTO>> GetModelsAsync()
        {
            var result = new List<ModelDTO>();

            for (int i = 0; i < _count; i++)
                result.Add(new ModelDTO { Id = i, Name = $"{nameof(ModelDTO)} - {i}" });

            return await Task.FromResult(result);
        }

        async public Task<IEnumerable<ModelFamilyDTO>> GetModelFamiliesAsync()
        {
            var result = new List<ModelFamilyDTO>();

            for (int i = 0; i < _count; i++)
                result.Add(new ModelFamilyDTO { Id = i, Name = $"{nameof(ModelFamilyDTO)} - {i}" });

            return await Task.FromResult(result);
        }

        async public Task<IEnumerable<PhysicalModelDTO>> GetPhysicalModelsAsync()
        {
            var result = new List<PhysicalModelDTO>();

            for (int i = 0; i < _count; i++)
                result.Add(new PhysicalModelDTO { Id = i, Name = $"{nameof(PhysicalModelDTO)} - {i}" });

            return await Task.FromResult(result);
        }

        async public Task<IEnumerable<OptionDTO>> GetOptionsAsync()
        {
            var result = new List<OptionDTO>();

            for (int i = 0; i < _count; i++)
                result.Add(new OptionDTO { Id = i, Name = $"{nameof(OptionDTO)} - {i}" });

            return await Task.FromResult(result);
        }

        async public Task<IEnumerable<KitDTO>> GetKitsAsync()
        {
            var result = new List<KitDTO>();

            for (int i = 0; i < _count; i++)
                result.Add(new KitDTO { Id = i, Name = $"{nameof(KitDTO)} - {i}" });

            return await Task.FromResult(result);
        }

        async public Task<IEnumerable<DistributorDTO>> GetDistributorsAsync()
        {
            var result = new List<DistributorDTO>();

            for (int i = 0; i < _count; i++)
                result.Add(new DistributorDTO { Id = i, Name = $"{nameof(DistributorDTO)} - {i}" });

            return await Task.FromResult(result);
        }

        async public Task<IEnumerable<CertifierDTO>> GetCertifiersAsync()
        {
            var result = new List<CertifierDTO>();

            for (int i = 0; i < _count; i++)
                result.Add(new CertifierDTO { Id = i, Name = $"{nameof(CertifierDTO)} - {i}" });

            return await Task.FromResult(result);
        }

        async public Task<IEnumerable<UserLevelDTO>> GetUsersAsync()
        {
            var result = new List<UserLevelDTO>();
            var users = Enum.GetValues(typeof(UserLevelDTO));

            foreach (int user in users)
                result.Add((UserLevelDTO)user);

            return await Task.FromResult(result);
        }
    }
}