package com.example.enums;

import tkmallcoo.mybatis.Enum.IEnum;


public enum GenderEnum implements IEnum{
    Male(3,"男"),
    Female(4,"女");

    private int code;
    private String desc;

    GenderEnum(int code,String desc) {
        this.code = code;
        this.desc = desc;
    }
    @Override
    public int getCode()
        {
        return code;
    }
}

