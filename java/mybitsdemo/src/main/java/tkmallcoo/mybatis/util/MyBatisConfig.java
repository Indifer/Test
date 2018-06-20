package tkmallcoo.mybatis.util;

import org.apache.ibatis.session.SqlSessionFactory;
import org.apache.ibatis.type.TypeHandlerRegistry;
import org.mybatis.spring.SqlSessionFactoryBean;
import org.springframework.boot.context.properties.ConfigurationProperties;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.core.io.support.PathMatchingResourcePatternResolver;
import org.springframework.core.io.support.ResourcePatternResolver;
import tkmallcoo.mybatis.Enum.IEnum;

import javax.sql.DataSource;
import java.util.List;

@Configuration
@ConfigurationProperties(prefix = "mybatis")
public class MyBatisConfig {

    private String configLocation;

    private String mapperLocations;

    @Bean
    public SqlSessionFactory sqlSessionFactory(DataSource dataSource, ResourcesUtil resourcesUtil) throws Exception {
        SqlSessionFactoryBean factory = new SqlSessionFactoryBean();
        factory.setDataSource(dataSource);


        // 设置配置文件地址
        ResourcePatternResolver resolver = new PathMatchingResourcePatternResolver();
        factory.setConfigLocation(resolver.getResource(configLocation));
        factory.setMapperLocations(resolver.getResources(mapperLocations));


        SqlSessionFactory sqlSessionFactory = factory.getObject();

        // -----------  动态加载实现IEnum接口的枚举，使用CodeEnumTypeHandler转换器

        // 取得类型转换注册器
        TypeHandlerRegistry typeHandlerRegistry = sqlSessionFactory.getConfiguration().getTypeHandlerRegistry();


        // 扫描所有实体类
        List<String> classNames = resourcesUtil.list("com/example", "/**/entity");

        for (String className : classNames) {
            // 处理路径成为类名
            className = className.replace('/', '.').replaceAll("\\.class", "");
            // 取得Class
            Class<?> aClass = Class.forName(className, false, getClass().getClassLoader());

            // 判断是否实现了BaseCodeEnum接口
            if (aClass.isEnum() && IEnum.class.isAssignableFrom(aClass)) {
                // 注册
                typeHandlerRegistry.register(className, "org.apache.ibatis.type.EnumOrdinalTypeHandler");
            }
        }

        // --------------- end

        return sqlSessionFactory;
    }

    public String getConfigLocation() {
        return configLocation;
    }

    public void setConfigLocation(String configLocation) {
        this.configLocation = configLocation;
    }

    public String getMapperLocations() {
        return mapperLocations;
    }

    public void setMapperLocations(String mapperLocations) {
        this.mapperLocations = mapperLocations;
    }
}