namespace WebApiOpenApi;

public static class SecurityHeadersDefinitionsAPI
{
    public static HeaderPolicyCollection GetHeaderPolicyCollection()
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
            builder.AddImgSrc().Self().From("data:");
            builder.AddFormAction().Self();
            builder.AddFontSrc().Self();
            builder.AddStyleSrc().Self().UnsafeInline();
            builder.AddScriptSrc().Self().UnsafeInline(); //.WithNonce();
            builder.AddBaseUri().Self();
            builder.AddFrameAncestors().None();
        });

        return policy;
    }
}
