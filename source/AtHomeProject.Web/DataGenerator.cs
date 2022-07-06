using System;
using System.Threading.Tasks;
using AtHomeProject.Data.Entities;
using AtHomeProject.Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Semver;

namespace AtHomeProject.Web
{
    public static class DataGenerator
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

            // Look for any department.
            if ((await unitOfWork.Device.CountAsync) > 0)
                return; // Data was already seeded

            await unitOfWork.Device.InsertRangeAsync(
                new Device
                {
                    SerialNumber = "0d0f40f0acb74bf0958c6c6c2a7e6f1f",
                    SecretKey = "fff754c711b34ccd9bf1547f2ea96049",
                    FirmwareVersion = SemVersion.Parse("1.0.0", SemVersionStyles.Strict)
                },
                new Device
                {
                    SerialNumber = "02488664b3dd433ba0ab64ba84b9539c",
                    SecretKey = "d7c8b47a619c442da8e918b875ea3e5c",
                    FirmwareVersion = SemVersion.Parse("1.0.0", SemVersionStyles.Strict)
                },
                new Device
                {
                    SerialNumber = "ea9c98ed90df4d2686d1b57264e8159e",
                    SecretKey = "ad5f8a5dffc542ef8417230f090a4b05",
                    FirmwareVersion = SemVersion.Parse("1.0.0", SemVersionStyles.Strict)
                },
                new Device
                {
                    SerialNumber = "e662a95e4a3245df8f13f6ab09384575",
                    SecretKey = "3a2e80528b2244d08cb7b4b0f2335b5d",
                    FirmwareVersion = SemVersion.Parse("1.0.0", SemVersionStyles.Strict)
                },
                new Device
                {
                    SerialNumber = "3f70d60576d64fba88716d382556e3d0",
                    SecretKey = "7e5958c36a084b06bcf4fc16a0e5ca3a",
                    FirmwareVersion = SemVersion.Parse("1.0.0", SemVersionStyles.Strict)
                }
            );

            await unitOfWork.SaveAsync();
        }
    }
}
