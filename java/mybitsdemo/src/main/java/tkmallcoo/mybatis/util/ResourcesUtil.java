package tkmallcoo.mybatis.util;


import org.springframework.core.io.Resource;
import org.springframework.core.io.support.PathMatchingResourcePatternResolver;
import org.springframework.core.io.support.ResourcePatternResolver;
import org.springframework.stereotype.Component;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

@Component
public class ResourcesUtil {

    private final ResourcePatternResolver resourceResolver;

    public ResourcesUtil() {
        this.resourceResolver = new PathMatchingResourcePatternResolver(getClass().getClassLoader());
    }

    /**
     * 返回路径下所有class
     *
     * @param rootPath        根路径
     * @param locationPattern 位置表达式
     * @return
     * @throws IOException
     */
    public List<String> list(String rootPath, String locationPattern) throws IOException {
        Resource[] resources = resourceResolver.getResources("classpath*:" + rootPath + locationPattern + "/**/*.class");
        List<String> resourcePaths = new ArrayList<>();
        for (Resource resource : resources) {
            //resourcePaths.add(preserveSubpackageName(resource.getURI(), rootPath));
            resourcePaths.add(resource.getURI().toString());
        }
        return resourcePaths;
    }
}
