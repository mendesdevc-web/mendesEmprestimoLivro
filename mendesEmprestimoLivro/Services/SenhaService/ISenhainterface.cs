namespace mendesEmprestimoLivro.Services.SenhaService
{
    public interface ISenhainterface
    {
        void CriarSenhaHash(string senha, out byte[] senhaHash, out byte[] senhaSalt);
        bool VerificaSenha(string senha, byte[] senhaHash, byte[] senhaSalt);

    }
}
