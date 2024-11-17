using System.Diagnostics.CodeAnalysis;
namespace Application.Common.Constants
{
    [ExcludeFromCodeCoverage]
    public static class ApiResponseMessages
    {
        public const string YouDoNotHavePermission = "Você não tem permissão.";
        public const string DocumentAlreadyRegistered = "CNPJ já cadastrado.";
        public const string UrlAlreadyUsed = "Essa url já está sendo utilizada.";
        public const string UserIdRequired = "O id do usuário é obrigatório.";
        public const string PasswordMostHaveAtLeastOneNumber = "A senha tem que ter pelo menos um número.";
        public const string PasswordMostHaveAtLeastOneUpperLetter = "A senha tem que ter pelo menos uma letra maiúscula.";
        public const string PasswordMostHaveAtLeastOneLowerLetter = "A senha tem que ter pelo menos uma letra minúscula.";
        public const string PasswordMostHaveAtLeastCharactersLong = "A senha tem que ter pelo menos 8 caracteres.";
        public const string PasswordMostHaveAtLeastSpecialCaracter = "A senha tem que ter pelo um caracter especial.";
        public const string PasswordAreNotTheSame = "As senhas não são iguais.";
        public const string UnableToaAcceptYouRequest = "Não foi possível aceitar sua solicitação, tente novamente mais tarde.";
        public const string PasswordOrEmailInvalid = "E-mail e/ou senha inválidos.";
        public const string UserDisabled = "Usuário desativado.";
        public const string UserNotFound = "Usuário não encontrado.";
        public const string InvalidActualPassword = "Senha atual inválida.";
        public const string AccessDenied = "Você não tem permissão para fazer essa ação.";
        public const string ValidationFails = "Uma ou mais validações falharam";
        public const string EmailRequired = "O e-mail é obrigatório";
        public const string TokenExpired = "Token Expirado.";
        public const string TenantIdRequired = "O id da company é obrigatório.";
        public const string CompanyNotFound = "Empresa não encontrada";
        public const string EnterAName = "Você tem que digitar um nome.";
        public const string EnterAPassword = "Você tem que digitar uma senha.";
        public const string InvalidEmail = "E-mail inválido.";
        public const string AllowAccessLikeEvaluator = "Permite acessar a plataforma como avaliador";
        public const string AllowAccessLikeAdministrator = "Permite acessar a plataforma como administrador";
        public const string PostNotFound = "Post não encontrado.";
        public const string SomeValidationsFail ="Algumas validações falharam.";
        public const string ErrorOccuredMessageNotCatched = "Erro ocorrido, mensagem não capturada.";
        
        public const string InvalidDocument = "CNPJ inválido.";
        public const string InvalidTabBrowserTitle = "Titulo de exibição da aba do navegador inválido";
        public const string InvalidUrl = "Url inválida. Url só podem conter letras minúsculas, números e hifens. Eles devem começar e terminar com uma letra ou número.";
        
        public const string EnterACompanyName = "Você tem que digitar a razão social.";
        public const string EnterATradingName = "Você tem que digitar o nome fantasia.";
        public const string EnterADocument = "Você tem que digitar um cnpj.";
        public const string EnterAUrl = "Você tem que digitar uma url.";
        public const string EnterAPrimaryColor = "Você tem que informar a cor primaria.";
        public const string InvalidPrimaryColor = "Cor primaria inválida. Padrão: #000000.";
        public const string EnterAButtonColor = "Você tem que informar a cor dos botões.";
        public const string InvalidButtonColor = "Cor dos botões inválida. Padrão: #000000.";

        public static string EmailAlreadyRegistered(string email)
        {
            return $"E-mail '{email}' já cadastrado.";
        }
    }
}
