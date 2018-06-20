package springconsulservertest.springconsulservertest;

import org.apache.logging.log4j.message.StringFormattedMessage;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.cloud.client.discovery.EnableDiscoveryClient;
import org.springframework.context.annotation.Configuration;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.text.MessageFormat;

@SpringBootApplication
@EnableDiscoveryClient
@RestController
@Configuration
public class SpringConsulserverTestApplication {

	@Value("${server.port}")
	private String port;

	@RequestMapping("/home")
	public String home(String name){

		return MessageFormat.format("hello {0}, ip:{1}", name, port);

	}

	public static void main(String[] args) {

//        System.setProperty("http.proxyHost", "localhost");
//        System.setProperty("http.proxyPort", "8888");

		SpringApplication.run(SpringConsulserverTestApplication.class, args);
	}


}
