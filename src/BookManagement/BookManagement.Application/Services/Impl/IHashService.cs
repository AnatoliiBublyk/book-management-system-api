namespace BookManagement.Application.Services.Impl;

public interface IHashService
{
    public bool VerifyHash(string input, string hash);
    public string GetHash(string input);
}