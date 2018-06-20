package springeurekaservertest.springeurekaservertest;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.cloud.netflix.eureka.server.EnableEurekaServer;

@SpringBootApplication
@EnableEurekaServer
public class SpringEurekaserverTestApplication {

	public static void main(String[] args) {
		SpringApplication.run(SpringEurekaserverTestApplication.class, args);
	}
}
