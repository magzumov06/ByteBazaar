﻿namespace Infrastructure.Services.HashServices;

public interface IHashService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
    Task<string> GenerateRandomCode(int length);
}