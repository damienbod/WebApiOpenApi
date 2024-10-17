namespace WebApiOpenApi;

public static class SecurityHeadersDefinitionsAPI
{
    public static HeaderPolicyCollection GetHeaderPolicyCollection(bool isDev)
    {
        var policy = new HeaderPolicyCollection()
            .AddFrameOptionsDeny()
            .AddContentTypeOptionsNoSniff()
            .AddReferrerPolicyStrictOriginWhenCrossOrigin()
            .AddCrossOriginOpenerPolicy(builder => builder.SameOrigin())
            .AddCrossOriginEmbedderPolicy(builder => builder.RequireCorp())
            .AddCrossOriginResourcePolicy(builder => builder.SameOrigin())
            .RemoveServerHeader()
            .AddPermissionsPolicyWithDefaultSecureDirectives();

        policy.AddContentSecurityPolicy(builder =>
        {
            builder.AddObjectSrc().None();
            builder.AddBlockAllMixedContent();
            builder.AddImgSrc().None();
            builder.AddFormAction().None();
            builder.AddFontSrc().None();
            builder.AddStyleSrc().None();
            builder.AddScriptSrc().None();
            builder.AddBaseUri().Self();
            builder.AddFrameAncestors().None();
            builder.AddCustomDirective("require-trusted-types-for", "'script'");
        });

        if (!isDev)
        {
            // maxage = one year in seconds
            policy.AddStrictTransportSecurityMaxAgeIncludeSubDomains(maxAgeInSeconds: 60 * 60 * 24 * 365);
        }

        return policy;
    }
}
