
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HH_RU_ParserService.Models;

public sealed record Item(
    [property: JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("id")] string Id,
    [property: JsonProperty("premium", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("premium")] bool? Premium,
    [property: JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("name")] string Name,
    [property: JsonProperty("department", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("department")] object Department,
    [property: JsonProperty("has_test", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("has_test")] bool? HasTest,
    [property: JsonProperty("response_letter_required", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("response_letter_required")] bool? ResponseLetterRequired,
    [property: JsonProperty("area", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("area")] Area Area,
    [property: JsonProperty("salary", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("salary")] Salary Salary,
    [property: JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("type")] Type Type,
    [property: JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("address")] Address Address,
    [property: JsonProperty("response_url", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("response_url")] object ResponseUrl,
    [property: JsonProperty("sort_point_distance", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("sort_point_distance")] object SortPointDistance,
    [property: JsonProperty("published_at", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("published_at")] string? PublishedAt,
    [property: JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("created_at")] string? CreatedAt,
    [property: JsonProperty("archived", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("archived")] bool? Archived,
    [property: JsonProperty("apply_alternate_url", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("apply_alternate_url")] string ApplyAlternateUrl,
    [property: JsonProperty("show_logo_in_search", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("show_logo_in_search")] bool? ShowLogoInSearch,
    [property: JsonProperty("insider_interview", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("insider_interview")] InsiderInterview InsiderInterview,
    [property: JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("url")] string Url,
    [property: JsonProperty("alternate_url", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("alternate_url")] string AlternateUrl,
    [property: JsonProperty("relations", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("relations")] IReadOnlyList<object> Relations,
    [property: JsonProperty("employer", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("employer")] Employer Employer,
    [property: JsonProperty("snippet", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("snippet")] Snippet Snippet,
    [property: JsonProperty("contacts", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("contacts")] object Contacts,
    [property: JsonProperty("schedule", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("schedule")] Schedule Schedule,
    [property: JsonProperty("working_days", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("working_days")] IReadOnlyList<object> WorkingDays,
    [property: JsonProperty("working_time_intervals", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("working_time_intervals")] IReadOnlyList<object> WorkingTimeIntervals,
    [property: JsonProperty("working_time_modes", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("working_time_modes")] IReadOnlyList<object> WorkingTimeModes,
    [property: JsonProperty("accept_temporary", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("accept_temporary")] bool? AcceptTemporary,
    [property: JsonProperty("professional_roles", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("professional_roles")] IReadOnlyList<ProfessionalRole> ProfessionalRoles,
    [property: JsonProperty("accept_incomplete_resumes", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("accept_incomplete_resumes")] bool? AcceptIncompleteResumes,
    [property: JsonProperty("experience", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("experience")] Experience Experience,
    [property: JsonProperty("employment", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("employment")] Employment Employment,
    [property: JsonProperty("adv_response_url", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("adv_response_url")] object AdvResponseUrl,
    [property: JsonProperty("is_adv_vacancy", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("is_adv_vacancy")] bool? IsAdvVacancy,
    [property: JsonProperty("adv_context", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("adv_context")] object AdvContext,
    [property: JsonProperty("branding", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("branding")] Branding Branding
);