// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

namespace SuperComicLib.YamlXX;

public enum YamlXXParseError
{
    // No error
    Success = 0,

    /// <summary>
    /// 예기치 못한 문자를 발견함
    /// </summary>
    UnexpectedCharacter = -1,

    /// <summary>
    /// 예기치 못한 끝을 만났음
    /// </summary>
    UnexpectedEndOfStream = -2,
    
    /// <summary>
    /// YAML 에 잘못된 요소 유형이 있음
    /// </summary>
    InvalidYamlElementType = -3,
    
    /// <summary>
    /// Expression Block에서 지원되지 않는 리터럴 유형을 발견함
    /// </summary>
    ExprNotSupportedLiteralType = -4,
    
    /// <summary>
    /// Expression Block에서 문자열 리터럴가 닫히지 않음
    /// </summary>
    ExprStringLiteralNotClosed = -5,
    
    /// <summary>
    /// Expr 블록에서 예기치 못한 블록의 끝을 만남
    /// </summary>
    ExprUnexpectedEndOfBlock = -6,
    
    /// <summary>
    /// 해시 테이블 키가 비어있음
    /// </summary>
    ExprHashtableEmptyKey = -7,
    
    /// <summary>
    /// 해시 테이블에 지정된 키가 존재하지 않음
    /// </summary>
    ExprHashtableKeyNotFound = -8,
    
    /// <summary>
    /// 액션 목록에서 지정된 이름의 액션을 찾을 수 없음
    /// </summary>
    ExprActionNameNotFound = -9,
    
    /// <summary>
    /// 액션 이름이 유효하지 않거나, 비어있음
    /// </summary>
    ExprActionNameInvalidOrEmpty = -10
}
