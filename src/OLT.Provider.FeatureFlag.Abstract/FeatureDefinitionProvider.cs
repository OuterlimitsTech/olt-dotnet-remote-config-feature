using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.FeatureManagement;

namespace OLT.Provider.FeatureFlag.Abstract
{

    /// <summary>
    /// https://stackoverflow.com/questions/64911784/ifeaturedefinitionprovider-for-selecting-features-from-sql-server
    /// </summary>
    public class FeatureDefinitionProvider : IFeatureDefinitionProvider
    {
        private const string FirstFeatureName = "FirstFeature";
        private const string SecondFeatureName = "SecondFeature";
        private const string ThirdFeatureName = "ThirdFeature";

        public Task<FeatureDefinition> GetFeatureDefinitionAsync(string featureName)
        {
            // YOU SUPPOSEDLY GO ASYNCHRONOUSLY TO THE DATABASE AND TAKE THE GIVEN FEATURE PROPERTIES
            // NOTE: part of the following is dummy
            var featureDefinition = featureName switch // NOTE: I'm using new C# switch expression here
            {
                // let's say the feature is boolean and it is enabled
                FirstFeatureName => CreateEnabledFeatureDefinition(featureName),

                // let's say the feature is boolean and it is disabled
                SecondFeatureName => CreateDisabledFeatureDefinition(featureName),

                // let's say this one is a 50% percentage
                ThirdFeatureName => CreatePercentageFeatureDefinition(featureName, 50),

                _ => throw new NotSupportedException("The requested feature is not supported.")
            };

            return Task.FromResult(featureDefinition);
        }

        public async IAsyncEnumerable<FeatureDefinition> GetAllFeatureDefinitionsAsync()
        {
            foreach (var featureDefinition in new[]
            {
                await GetFeatureDefinitionAsync(FirstFeatureName),
                await GetFeatureDefinitionAsync(SecondFeatureName),
                await GetFeatureDefinitionAsync(ThirdFeatureName),
            })
            {
                yield return featureDefinition;
            }
        }

        private FeatureDefinition CreateEnabledFeatureDefinition(string featureName)
        {
            // NOTE: adding a filter configuration without configurations means enabled
            return new FeatureDefinition
            {
                Name = featureName,
                EnabledFor = new[]
                {
                    new FeatureFilterConfiguration
                    {
                        Name = "AlwaysOn"
                    }
                }
            };
        }

        private FeatureDefinition CreateDisabledFeatureDefinition(string featureName)
        {
            // NOTE: don't add any filter configuration as by default it is disabled
            return new FeatureDefinition
            {
                Name = featureName
            };
        }

        private FeatureDefinition CreatePercentageFeatureDefinition(string featureName, double percentage)
        {
            // NOTE: this one is a bit more complicated and could be connected to a percentage SQL server setting
            return new FeatureDefinition
            {
                Name = featureName,
                EnabledFor = new[]
                {
                    new FeatureFilterConfiguration
                    {
                        Name = "Percentage",
                        Parameters = new DoubleConfiguration(percentage)
                    }
                }
            };
        }
    }
}
