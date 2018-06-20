package com.example.model;

import lombok.*;
import lombok.experimental.Accessors;
import tkmallcoo.mybatis.Entity.IEntity;

import javax.persistence.*;
import java.util.Date;

@Getter
@Setter
@ToString
@NoArgsConstructor
@Accessors(chain = true)
public class User implements IEntity {
    @Id
    @GeneratedValue(generator = "JDBC")
    private Integer id;
    private String username;

    private String password;
    @Column(name="nick_name")
    private String nickname;
    private Integer sex;
    private Date register_date;

    public User(String username,String password,String nickname,Integer sex,Date register_date)
    {
        this.username=username;
        this.password=password;
        this.nickname=nickname;
        this.sex=sex;
        this.register_date=register_date;
    }
}
