using System.Diagnostics.CodeAnalysis;
namespace Application.Common.Constants
{
  [ExcludeFromCodeCoverage]
    public static class DownloadTemplate
    {
        public const string ManifestHtml = @"<!DOCTYPE html>
<html lang=""en"">
<head>
  <meta charset=""UTF-8"">
  <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
  <link rel=""preconnect"" href=""https://fonts.googleapis.com"">
  <link rel=""preconnect"" href=""https://fonts.gstatic.com"" crossorigin>
  <link href=""https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700&display=swap"" rel=""stylesheet"">
  <title>Document</title>
  <style>
   * {
      box-sizing: border-box;
      font-family: 'Inter', Helvetica, sans-serif;
    }

    a {
      color: #01602a;
      text-decoration: none;
    }

    body {
      background-color: #FAFAFA;
      color: #333333;
    }

    main {
      width: 640px;
      margin: 0 auto;
      padding: 38px;
    }

    .cabecalho {
      width: 100%;
      display: flex;
      background: #01602a;
      align-items: center;
      padding: 10px;
      border-radius: 5px;
      justify-content: center;
    }

    .detalhes h2 {
      font-size: 20px;
      margin-top: 24px;
    }

    .detalhe {
      display: flex;
      width: 100%;
      justify-content: space-between;
    }

   .detalhe strong {
      font-size: 14px;
    }

    .detalhe span {
      font-size: 14px;
    }

    .ajuda {
      background-color:rgba(245, 245, 245, 1);
      padding: 32px 24px;
      border-radius: 16px;
      margin: 32px 0;
    }

    .subtitle {
      font-size: 18px;
      margin: 0px;
      padding: 0px;
    }

    .title-duvidas{
      font-size: 14px;
      font-weight: 700;
      line-height: 22.4px;
    }

    .text {
      font-size: 14px;
      font-weight: 400;
      line-height: 22.4px;
    }

    .text-duvidas {
      font-size: 12px;
      font-weight: 400;
      line-height: 22.4px;
    }

    .protocolo {
      background-color: #DDD;
      padding: 16px 64px;
      width: 424px;
      margin: 0 auto;
      display: flex;
      flex-direction: column;
      gap: 12px;
      border-radius: 8px;
    }
    
    .protocolo div {
      display: grid;
      grid-template-columns: repeat(3, 1fr);
    }

    .protocolo table {
      line-height: 24px;
    }

    .duvidas h3 {
      margin: 0;
    }

    .duvidas-area{
      display: flex;
      gap: 20px;
    }

    .area50{
      display: block;
      justify-content: center;
      width: 50%;
    }

    .contato-area{
      display: flex;
      align-items: center;
      margin-bottom: 16px;
      margin-left: 20px;
    }

    h4{
      margin: 0;
      font-weight: 400;
    }

    .footer {
      background-color: #F5F5F5; 
      padding: 16px; 
      margin-top: 42px; 
      border-radius: 16px;
    }
  </style>
</head>
<body>
  <main>
    <div class=""relato"">
      <div class=""cabecalho"">
        <a href=""[api_url]""><img style=""width: 90px;"" src=""[logo]"" /></a>
      </div>

      <div class=""detalhes"">
        <h2>Relato [channel]</h2>
        <div class=""detalhe"">
          <div>
            <strong>RELATOR:</strong>
            <span>Anônimo</span>
          </div>

          <div style=""flex: 1""></div>

          <div>
            <strong>ENVIADO EM:</strong>
            <span>[send_date]</span>
          </div>
        </header>
      </div>      
    </div>

    <div class=""ajuda"">
      <h3 class=""subtitle"">Como consultar?</h3>
      <p class=""text"">
        Para acompanhar o andamento do seu relato, ver a resposta da empresa e fazer complementos ao manifesto original, 
        acesse o site: <a href=""[api_url]/consult"">[api_url]/consult</a> e utilize o 
        protocolo e senha para realizar o login e visualizar as informações.
      </p>

      <div class=""protocolo"">
        <table width=""100%"">
          <tr>
            <td>Protocolo: </td>
            <td><strong>[protocol]</strong></td>
          </tr>
          <tr>
            <td>Senha: </td>
            <td><strong>[password]</strong></td>
          </tr>
        </table>
      </div>
    </div>

  <!--

    <div class=""duvidas"">      
      <div class=""duvidas-area"">
        <div class=""area50"">
          <h3 class=""title-duvidas"">Dúvidas</h3>
          <p class=""text-duvidas"" style=""text-align: start;"">
            No caso de dúvidas, fique à vontade para entrar em contato conosco 
            através de algum dos seguintes canais disponíveis. 
          </p>
        </div>
        <div class=""area50 direita"">
          <div class=""contato-area"">
            <img style=""vertical-align: middle; margin-right: 16px;"" height=""24px""src=""[app_base_url]/social_media.png""/>
            <h4 style=""vertical-align: middle;font-size:12px;"">@youin.digital</h4>
          </div>
          <div class=""contato-area"">
            <img style=""vertical-align: middle; margin-right: 16px;"" height=""24px""src=""[app_base_url]/message.png"" />
            <h4 style=""vertical-align: middle;font-size:12px;"">(31) 98602-0495</h4>
          </div>
          <div class=""contato-area"">
            <img style=""vertical-align: middle; margin-right: 16px;"" height=""24px""src=""[app_base_url]/mail_outline.png"" />
            <h4 style=""vertical-align: middle;font-size:12px;"">contato@youin.digital</h4>
          </div>
        </div>
      </div>
    </div>

    -->

    <div class=""footer"">
      <table border=""0"" width=""100%"">
        <tbody>
          <tr>
            <td>
              <a href=""https://youin.digital""><img style=""width: 80px;"" src=""[app_base_url]/logo-blue.png"" /></a>
            </td>
            <td style=""width: 320px; font-size:12px; color: #B0B0B0; line-height: 19.2px;"">
              © 2023 Rhitmo Soluções em tecnologia LTDA. <br>
              Todos os direitos reservados.
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </main>
</body>
</html>";
    }
}
