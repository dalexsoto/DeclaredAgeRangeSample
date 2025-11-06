using System;
using ObjCRuntime;

namespace DeclaredAgeRangeWrapper
{
    [Native]
    public enum MyAgeRangeDeclaration : long
    {
        SelfDeclared = 0,
        GuardianDeclared = 1,
        Unknown = 2
    }

    [Native]
    public enum MyAgeRangeResponseType : long
    {
        Sharing = 0,
        DeclinedSharing = 1
    }
}