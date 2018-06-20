package hello.formats;

import org.springframework.format.Formatter;

import java.text.ParseException;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.Locale;

/**
 * LocalDate格式化
 */
public class LocalDateFormatter implements Formatter<LocalDateTime> {

    public static final String LocalDate_Formatter = "yyyy/MM/dd HH:mm:ss";

    @Override
    public LocalDateTime parse(String s, Locale locale) throws ParseException {
        return LocalDateTime.parse(s);
    }

    @Override
    public String print(LocalDateTime localDateTime, Locale locale) {
        return localDateTime.format(DateTimeFormatter.ofPattern(LocalDate_Formatter));
    }
}
