using Boilerplate.Contracts;
using Boilerplate.Entities.DBModels;
using Microsoft.Extensions.Configuration;

namespace Boilerplate.Service.ValidationHelpers;
public class ValidationHelper
{
    private readonly IConfiguration _configuration;
    private readonly string _schemaFilePath;
    public ValidationHelper(IConfiguration configuration)
    {
        _schemaFilePath = configuration["ValidationSchema:FilePath"];

        if (string.IsNullOrEmpty(_schemaFilePath))
        {
            throw new Exception("Validation schema file path is not configured in appsettings.json");
        }

        if (!File.Exists(_schemaFilePath))
        {
            throw new FileNotFoundException($"Validation schema file not found at {_schemaFilePath}");
        }
    }

    public void ValidateModel(MasterEntryModel model, string schemaKey)
    {             
        var schemaContent = File.ReadAllText(_schemaFilePath);
        var schema = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(schemaContent);
      
        var rules = schema["ValidationSchemas"]?[schemaKey];
        if (rules == null) throw new Exception($"No validation rules found for the entity: {schemaKey}");

        var queryParamsRules = rules["QueryParams"];
        var whereParamsRules = rules["WhereParams"];
       
        ValidateProperty(
            propertyValue: model.TableName,
            propertyRules: rules["TableName"],
            propertyName: "TableName"
        );
        
        ValidateProperty(
            propertyValue: model.ColumnNames,
            propertyRules: rules["ColumnNames"],
            propertyName: "ColumnNames",
            customValidators: new List<Func<string, bool>>
            {
            value => value.Split(',').Length <= (int)(rules["ColumnNames"]?["MaxCount"] ?? int.MaxValue)
            },
            customErrorMessages: new List<string>
            {
            $"ColumnNames exceeds max count of {rules["ColumnNames"]?["MaxCount"]}."
            }
        );
        
        ValidateParameters(
            parameters: model.QueryParams,
            paramRules: queryParamsRules,
            paramType: "QueryParams"
        );
        
        ValidateParameters(
            parameters: model.WhereParams,
            paramRules: whereParamsRules,
            paramType: "WhereParams"
        );
    }

    private void ValidateProperty(
        string propertyValue,
        dynamic propertyRules,
        string propertyName,
        List<Func<string, bool>>? customValidators = null,
        List<string>? customErrorMessages = null
    )
    {
        if (propertyRules == null) return;
        
        if (propertyRules["Required"] != null && (bool)propertyRules["Required"] && string.IsNullOrWhiteSpace(propertyValue))
        {
            throw new Exception($"{propertyName} is required.");
        }
        
        if (propertyRules["MaxLength"] != null && propertyValue?.Length > (int)propertyRules["MaxLength"])
        {
            throw new Exception($"{propertyName} exceeds max length of {propertyRules["MaxLength"]}.");
        }

        if (customValidators != null && customErrorMessages != null)
        {
            for (int i = 0; i < customValidators.Count; i++)
            {
                if (!customValidators[i](propertyValue))
                {
                    throw new Exception(customErrorMessages[i]);
                }
            }
        }
    }

    private void ValidateParameters(
        object? parameters,
        dynamic paramRules,
        string paramType
    )
    {
        if (parameters == null || paramRules == null) return;

        string jsonString = parameters.ToString();

        Dictionary<string, object> paramCollection = null;

        try
        {
            paramCollection = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to deserialize {paramType}: {ex.Message}");
        }

        if (paramCollection == null || !paramCollection.Any())
        {
            throw new Exception($"{paramType} is empty or invalid.");
        }

        foreach (var param in paramCollection)
        {
            string paramName = param.Key;
            string paramValue = param.Value?.ToString();
            var paramRule = paramRules?[paramName];

            if (paramRule == null) continue;

            // Validate Required
            if (paramRule["Required"] != null && (bool)paramRule["Required"] && string.IsNullOrWhiteSpace(paramValue))
            {
                throw new Exception($"{paramName} in {paramType} is required.");
            }

            // Validate MaxLength
            if (paramRule["MaxLength"] != null && paramValue?.Length > (int)paramRule["MaxLength"])
            {
                throw new Exception($"{paramName} in {paramType} exceeds max length of {paramRule["MaxLength"]}.");
            }
        }
    }
}
