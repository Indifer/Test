package com.example;

//特别注意，下面的是 tk.MapperScan

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import sun.security.krb5.Config;
import tk.mybatis.mapper.mapperhelper.MapperHelper;
import tk.mybatis.spring.annotation.MapperScan;

/**
 * @author liuzh
 * @since 2015-12-12 18:22
 */
//@Controller
//@EnableWebMvc
@SpringBootApplication
@MapperScan(basePackages = "com.example.mapper")
public class Application{ //extends WebMvcConfigurerAdapter implements CommandLineRunner {
    //private Logger logger = LoggerFactory.getLogger(Application.class);

    public static void main(String[] args) {
        SpringApplication.run(Application.class, args);
    }


}
