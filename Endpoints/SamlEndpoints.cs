using System.IO.Compression;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using MockServer.Data;

namespace MockServer.Endpoints;

public static class SamlEndpoints
{
    private const string Issuer = "http://localhost:5001/sso";
    private const string FallbackAcsUrl = "http://localhost:3001/api/saml/login/callback";
    private static readonly string PrivateKeyPem = "-----BEGIN PRIVATE KEY-----\nMIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQChmQfSO5JY7Vr6\nvH0bU3MODzaaYIBXuNX9IDLHvag0f4e1ViH2ibKsbcZVlqq9yWHEsOzyREGHLdz7\n47iF9Mn62rNvkbNW4KuUJX8C2oYKPPw7fMilypjQ1HuoGP5dVOMO/12CyblpGRNK\nA5uDZVNwqVkU2+ZnBUHEFhK7nXcLGs9w0UAClaOxaqKfZIZRipfsjDebUVXCTU1Y\ncn0AGZLOS5ucEAbRg557DhmsluWbWtG4DdWRsEZAJESGy3AUQEKuEtNx0FY9ZhKh\nOuCnmcd7DDzjQ7XGIRfubLI4v1+kIIxuF5Njp2Dey/wMHYpXq4vUb+amR75puR7o\n1sPN3vATAgMBAAECggEANdknLIyST1TEqOHPdKAhqlEyek3m5pRepkso8LtHxjqq\n76fEiIFJC0FHQnUC71wgWqk0Q+Svy0pipYM0FVtFRPTEr0xnbO9P7X1zLfHAkb/R\ns0b5G5n/wzLTA+hUNqiXQwOgVvk+cgGblmHOXAITRGsCFrvSMokjaaKLWn510Tp1\nyWE8g/GRT1n0ohBZ9Cc7VA8Dh7+zXp6AEtVv8DVNWnlilsMehrRfLNjVNSepmwdI\npmxlCGCN9EBBiMeZbHKI4SoGYy6T8AMl0+4DhajglRztS0cOk1teUl0vMJ8hrLIh\nfogVPecSyVc0tbtPlSyQ9MkclG0CjaSPAm/2OeawYQKBgQDPrjbmX1Xv904NGgIL\nJbbohS3t3kcyjCzC7/WKLyshbQskoit7RBYlXa4yayU65qBxdydp81JG5sQlXOs/\n8wCKGbS1RQZwwpyEtWZ6wZumejSsWrNlOJihHrF0NfceVoZSTq+M8IHVraKC03wi\nRKK4oL1rzB1jE2yIhesn6JwC4QKBgQDHMg20sIADncCEMfBYeojueQwtZXtu7bsi\nNaRYJ5wpmyzjOB5nUg4GVQbV9Zh7iGCoCIbFl0GqlivcH2ZyxXH+n+FThvC4OuSu\nkrY8oKrhHEE7Rh9w9GLeQ9GFIg9LmnH21AfB67PAgzCQozFgqPxi0KRYH8WpVhY8\nE7NZ5mNFcwKBgQCbJxsKtpScRtS/wvtd2powUjbC63mjUMesBtsYli6RtUZK4wC7\nAV3OIpOeRR+2Rk/9FLiUVdzU7VgCVc3Go8N2aMfxWJT8LtgB6QeblAK1t7ycOtKl\nP0f/rs+B80MgHiIRYYmTMKa+vQ6Y0Gh/rWknp6Z73yzhNMSzpeDNmuDa4QKBgDvR\nsz389ySe3i6U1Kmtequq3FZRsIS6jcUTONxkYMg52nwN4UuD38RVCm18iKMobGZp\n0RdLeiPhXyCGqXx0AOzHVKKB1o5s2Tn3wxRn54kctBY5071XFs3KrgP+G3vH0Mug\nFwhMYTnB6/azm2N8u/Zs073HJNPaj2jRPb4UquEzAoGBAIeF2klCvUyE6xQmhVhx\nZ5qzjjNFbAiVcEr+o0mbxQOQSmzRAFeFoi4AhG3ICnPqdxoSXWaIxclgujjtNXeJ\nJgPx9w11RX6cLOIjpjxOZ95AtfwqgnXs086YFrJoD81ERQb+ZcH3bqzMSqVvvR6Q\nuWEfpPLxJoB8uwF1WLDwBhOe\n-----END PRIVATE KEY-----";
    private static readonly string CertPem = "-----BEGIN CERTIFICATE-----\nMIIDQTCCAimgAwIBAgIUPZP/yiEdXo0XhPS8vFcjNCsB1KMwDQYJKoZIhvcNAQEL\nBQAwMDEWMBQGA1UEAwwNTW9jayBTQU1MIElkUDEWMBQGA1UECgwNYm9hcmRpbmct\nbW9jazAeFw0yNjAzMjcxODAyMDNaFw0zNjAzMjQxODAyMDNaMDAxFjAUBgNVBAMM\nDU1vY2sgU0FNTCBJZFAxFjAUBgNVBAoMDWJvYXJkaW5nLW1vY2swggEiMA0GCSqG\nSIb3DQEBAQUAA4IBDwAwggEKAoIBAQChmQfSO5JY7Vr6vH0bU3MODzaaYIBXuNX9\nIDLHvag0f4e1ViH2ibKsbcZVlqq9yWHEsOzyREGHLdz747iF9Mn62rNvkbNW4KuU\nJX8C2oYKPPw7fMilypjQ1HuoGP5dVOMO/12CyblpGRNKA5uDZVNwqVkU2+ZnBUHE\nFhK7nXcLGs9w0UAClaOxaqKfZIZRipfsjDebUVXCTU1Ycn0AGZLOS5ucEAbRg557\nDhmsluWbWtG4DdWRsEZAJESGy3AUQEKuEtNx0FY9ZhKhOuCnmcd7DDzjQ7XGIRfu\nbLI4v1+kIIxuF5Njp2Dey/wMHYpXq4vUb+amR75puR7o1sPN3vATAgMBAAGjUzBR\nMB0GA1UdDgQWBBS53PgNuzEUTFoZL4Hh652harrycTAfBgNVHSMEGDAWgBS53PgN\nuzEUTFoZL4Hh652harrycTAPBgNVHRMBAf8EBTADAQH/MA0GCSqGSIb3DQEBCwUA\nA4IBAQBWz7GHcw4JQbCxgQLGucHZK3uiakWoR9xJ5lQSUaGIJqHesS26aQNofaXZ\n7hNiqjcRLs4o8Wb3JLIVYtYDTBWynHPXaYN4A4xiQHKJZ6rmvzNu8k2/36l8xZRG\nHlwfp86AWHUCFhw7//qYoPw2Mr2qwRWwmv5nimfvW9TJU/OXE3SfPfPxn5io0x7K\n6wOgEQVs9DGlFi73DvPEb9yC5xGkvgdG4wIADcjnWv4XDVJtTShCBSVQNe5kJjXL\nLvWyLIm9ScLMTTkOlitavsUyvI9Al3bOLTwKh/LTTkFQJsemoSA7ClYzvyTPiCNg\nkiRWY/FkY9f9rEcItOA4y+T69jfe\n-----END CERTIFICATE-----";

    public static void MapSamlEndpoints(this WebApplication app)
    {
        app.MapGet("/sso", (string? SAMLRequest, string? RelayState) =>
        {
            var (acsUrl, inResponseTo) = DecodeSamlRequest(SAMLRequest);
            return Results.Content(LoginPageHtml(acsUrl, RelayState ?? "", inResponseTo), "text/html");
        });

        app.MapPost("/sso", async (HttpContext ctx) =>
        {
            var form = await ctx.Request.ReadFormAsync();
            var username     = form["username"].ToString();
            var acsUrl       = form["acsUrl"].ToString();
            var relayState   = form["relayState"].ToString();
            var inResponseTo = form["inResponseTo"].ToString();
            var emp = MockStore.Employees.FirstOrDefault(e => e.LoginName == username)
                      ?? MockStore.Employees.First();
            var xml     = BuildSignedResponse(emp.Email!, emp.LoginName!, emp.Givenname!, emp.Lastname!, acsUrl, inResponseTo);
            var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(xml));
            return Results.Content(AutoSubmitHtml(acsUrl, encoded, relayState), "text/html");
        });

        app.MapGet("/sso/cert", () => Results.Text(CertPem, "text/plain"));
    }

    private static (string acsUrl, string inResponseTo) DecodeSamlRequest(string? samlRequest)
    {
        try
        {
            if (string.IsNullOrEmpty(samlRequest)) return (FallbackAcsUrl, "_mock");
            var bytes = Convert.FromBase64String(samlRequest);
            using var ms = new MemoryStream(bytes);
            using var deflate = new DeflateStream(ms, CompressionMode.Decompress);
            using var reader = new StreamReader(deflate, Encoding.UTF8);
            var xml = reader.ReadToEnd();
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            var root = doc.DocumentElement;
            var acs = root?.GetAttribute("AssertionConsumerServiceURL") ?? FallbackAcsUrl;
            var id  = root?.GetAttribute("ID") ?? "_mock";
            return (string.IsNullOrEmpty(acs) ? FallbackAcsUrl : acs, string.IsNullOrEmpty(id) ? "_mock" : id);
        }
        catch { return (FallbackAcsUrl, "_mock"); }
    }

    private static string BuildSignedResponse(string email, string username, string givenName, string surname, string acsUrl, string inResponseTo)
    {
        var now = DateTime.UtcNow;
        var rid = "_r" + Guid.NewGuid().ToString("N");
        var aid = "_a" + Guid.NewGuid().ToString("N");
        var ts  = now.ToString("yyyy-MM-ddTHH:mm:ssZ");
        var nb  = now.AddMinutes(-5).ToString("yyyy-MM-ddTHH:mm:ssZ");
        var na  = now.AddHours(1).ToString("yyyy-MM-ddTHH:mm:ssZ");

        var raw = "<samlp:Response"
            + " xmlns:samlp=\"urn:oasis:names:tc:SAML:2.0:protocol\""
            + " xmlns:saml=\"urn:oasis:names:tc:SAML:2.0:assertion\""
            + $" ID=\"{rid}\" Version=\"2.0\" IssueInstant=\"{ts}\" Destination=\"{acsUrl}\" InResponseTo=\"{inResponseTo}\">"
            + $"<saml:Issuer>{Issuer}</saml:Issuer>"
            + "<samlp:Status><samlp:StatusCode Value=\"urn:oasis:names:tc:SAML:2.0:status:Success\"/></samlp:Status>"
            + $"<saml:Assertion ID=\"{aid}\" Version=\"2.0\" IssueInstant=\"{ts}\" xmlns:saml=\"urn:oasis:names:tc:SAML:2.0:assertion\">"
            + $"<saml:Issuer>{Issuer}</saml:Issuer>"
            + $"<saml:Subject><saml:NameID Format=\"urn:oasis:names:tc:SAML:1.1:nameid-format:emailAddress\">{email}</saml:NameID>"
            + $"<saml:SubjectConfirmation Method=\"urn:oasis:names:tc:SAML:2.0:cm:bearer\"><saml:SubjectConfirmationData NotOnOrAfter=\"{na}\" Recipient=\"{acsUrl}\" InResponseTo=\"{inResponseTo}\"/></saml:SubjectConfirmation></saml:Subject>"
            + $"<saml:Conditions NotBefore=\"{nb}\" NotOnOrAfter=\"{na}\"><saml:AudienceRestriction><saml:Audience>passport-saml</saml:Audience></saml:AudienceRestriction></saml:Conditions>"
            + $"<saml:AuthnStatement AuthnInstant=\"{ts}\" SessionIndex=\"{aid}\"><saml:AuthnContext><saml:AuthnContextClassRef>urn:oasis:names:tc:SAML:2.0:ac:classes:Password</saml:AuthnContextClassRef></saml:AuthnContext></saml:AuthnStatement>"
            + "<saml:AttributeStatement>"
            + $"<saml:Attribute Name=\"uid\"><saml:AttributeValue>{username}</saml:AttributeValue></saml:Attribute>"
            + $"<saml:Attribute Name=\"givenname\"><saml:AttributeValue>{givenName}</saml:AttributeValue></saml:Attribute>"
            + $"<saml:Attribute Name=\"surname\"><saml:AttributeValue>{surname}</saml:AttributeValue></saml:Attribute>"
            + $"<saml:Attribute Name=\"groups\"><saml:AttributeValue>mock-users</saml:AttributeValue></saml:Attribute>"
            + "</saml:AttributeStatement>"
            + "</saml:Assertion></samlp:Response>";

        var xmlDoc = new XmlDocument { PreserveWhitespace = false };
        xmlDoc.LoadXml(raw);

        using var rsa = RSA.Create();
        rsa.ImportFromPem(PrivateKeyPem);
        var cert = X509Certificate2.CreateFromPem(CertPem);

        var signed = new SignedXml(xmlDoc);
        signed.SigningKey = rsa;
        signed.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;
        signed.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
        var reference = new Reference("#" + aid);
        reference.DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";
        reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
        reference.AddTransform(new XmlDsigExcC14NTransform());
        signed.AddReference(reference);
        signed.KeyInfo = new KeyInfo();
        signed.KeyInfo.AddClause(new KeyInfoX509Data(cert));
        signed.ComputeSignature();

        var sigEl = signed.GetXml();
        var nsMgr = new XmlNamespaceManager(xmlDoc.NameTable);
        nsMgr.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");
        var assertionEl = xmlDoc.SelectSingleNode($"//saml:Assertion[@ID='{aid}']", nsMgr)!;
        var issuerEl    = assertionEl.SelectSingleNode("saml:Issuer", nsMgr)!;
        assertionEl.InsertAfter(xmlDoc.ImportNode(sigEl, true), issuerEl);

        return xmlDoc.OuterXml;
    }

    private static string LoginPageHtml(string acsUrl, string relayState, string inResponseTo) =>
        "<!DOCTYPE html><html><head><meta charset=\"utf-8\"><title>Mock SAML IdP</title>"
        + "<style>body{font-family:sans-serif;background:#1a1a2e;color:#eee;display:flex;align-items:center;justify-content:center;min-height:100vh;margin:0}"
        + ".box{background:#16213e;padding:2rem;border-radius:12px;width:360px}"
        + "h1{font-size:1.2rem;margin-bottom:.3rem}p{color:#aaa;font-size:.85rem;margin-bottom:1.5rem}"
        + "button{width:100%;padding:.9rem;margin-bottom:.7rem;border:none;border-radius:8px;cursor:pointer;font-size:.95rem;text-align:left;background:#0f3460;color:#eee}"
        + "button:hover{background:#1a4a8a}.role{font-size:.75rem;color:#7eb8f7}</style></head>"
        + "<body><div class=\"box\"><h1>Mock SAML Login</h1><p>Välj persona att logga in som</p>"
        + $"<form method=\"post\" action=\"/sso\">"
        + $"<input type=\"hidden\" name=\"acsUrl\" value=\"{acsUrl}\">"
        + $"<input type=\"hidden\" name=\"relayState\" value=\"{relayState}\">"
        + $"<input type=\"hidden\" name=\"inResponseTo\" value=\"{inResponseTo}\">"
        + "<input type=\"hidden\" name=\"username\" id=\"u\" value=\"anna.lindgren\">"
        + "<button type=\"submit\" onclick=\"document.getElementById('u').value='anna.lindgren'\">Anna Lindgren<br><span class=\"role\">IT-chef · Manager</span></button>"
        + "<button type=\"submit\" onclick=\"document.getElementById('u').value='erik.johansson'\">Erik Johansson<br><span class=\"role\">Systemutvecklare · Ny medarbetare</span></button>"
        + "<button type=\"submit\" onclick=\"document.getElementById('u').value='sara.nilsson'\">Sara Nilsson<br><span class=\"role\">Delegationspersona</span></button>"
        + "</form></div></body></html>";

    private static string AutoSubmitHtml(string acsUrl, string samlResponse, string relayState) =>
        "<!DOCTYPE html><html><head><meta charset=\"utf-8\"><title>Redirecting...</title></head><body>"
        + $"<form id=\"f\" method=\"post\" action=\"{acsUrl}\">"
        + $"<input type=\"hidden\" name=\"SAMLResponse\" value=\"{samlResponse}\">"
        + $"<input type=\"hidden\" name=\"RelayState\" value=\"{relayState}\">"
        + "</form><script>document.getElementById('f').submit();</script></body></html>";
}
