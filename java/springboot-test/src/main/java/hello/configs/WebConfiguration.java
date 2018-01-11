package hello.configs;

import hello.formats.LocalDateFormatter;
import org.springframework.format.FormatterRegistry;
import org.springframework.web.servlet.config.annotation.WebMvcConfigurerAdapter;

import java.time.LocalDateTime;

public class WebConfiguration extends WebMvcConfigurerAdapter {

    @Override
    public void addFormatters(FormatterRegistry registry) {
        registry.addFormatterForFieldType(LocalDateTime.class, new LocalDateFormatter());
        super.addFormatters(registry);
    }
}
