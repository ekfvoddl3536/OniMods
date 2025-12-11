// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

#nullable enable
using System.Buffers;
using VYaml.Parser;

namespace SuperComicLib.YamlXX;

public static class PSLikeYaml
{
    public static void TryParseFromBytes(
        ReadOnlyMemory<byte> utf8RawYAMLText,
        ref StringInterpolatedScriptBlockHandler scriptBlockHandler)
    {
        TryParseFromSequence(new(utf8RawYAMLText), ref scriptBlockHandler);
    }
    
    public static void TryParseFromSequence(
        ReadOnlySequence<byte> utf8RawYAMLText, 
        ref StringInterpolatedScriptBlockHandler scriptBlockHandler)
    {
        var yaml = new YamlParser(utf8RawYAMLText);
        yaml.SkipAfter(ParseEventType.DocumentStart);
        var mark = yaml.CurrentMark;

        while (yaml.Read())
        {
            if (yaml.CurrentEventType == ParseEventType.MappingEnd)
            {
                break;
            }
            
            if (yaml.CurrentEventType != ParseEventType.Scalar)
            {
                // 잘못된 형식임. 키 값 모두 다 Scalar 여야함
                scriptBlockHandler.SetLastError(
                    "YAML key element must be of scalar type.", 
                    YamlXXParseError.InvalidYamlElementType, 
                    mark);
                break;
            }

            var key = yaml.GetScalarAsString()!;
            mark = yaml.CurrentMark;

            if (!yaml.Read())
            {
                // 첫번째 스칼라(키) 이후에 값이 필요함
                scriptBlockHandler.SetLastError(
                    "Parser encountered an unexpected end.", 
                    YamlXXParseError.UnexpectedEndOfStream,
                    mark);
                break;
            }
            
            if (yaml.GetScalarAsString() is not string value)
            {
                // 첫번째 스칼라의 다음에 이어나오는 건 반드시 스칼라여야함
                scriptBlockHandler.SetLastError(
                    "YAML/XE value  element must be of scalar type.", 
                    YamlXXParseError.InvalidYamlElementType,
                    mark);
                break;
            }
            
            var lastError = scriptBlockHandler.ParseAppend(value);
            if (lastError != YamlXXParseError.Success)
            {
                scriptBlockHandler.SetLastError(
                    "An error occurred while parsing the element value.",
                    lastError,
                    mark);
                break;
            }
            
            mark = yaml.CurrentMark;
            scriptBlockHandler.AddString(key);
        }
    }
}
