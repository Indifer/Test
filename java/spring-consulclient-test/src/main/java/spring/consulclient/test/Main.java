package spring.consulclient.test;

import com.orbitz.consul.Consul;
import com.orbitz.consul.HealthClient;
import com.orbitz.consul.KeyValueClient;
import com.orbitz.consul.async.ConsulResponseCallback;
import com.orbitz.consul.cache.ConsulCache;
import com.orbitz.consul.cache.ServiceHealthCache;
import com.orbitz.consul.cache.ServiceHealthKey;
import com.orbitz.consul.model.ConsulResponse;
import com.orbitz.consul.model.agent.Agent;
import com.orbitz.consul.model.health.ServiceHealth;
import com.orbitz.consul.model.kv.Value;
import com.orbitz.consul.option.QueryOptions;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.cloud.client.ServiceInstance;
import org.springframework.cloud.client.discovery.DiscoveryClient;
import org.springframework.cloud.client.discovery.EnableDiscoveryClient;
import org.springframework.cloud.netflix.feign.EnableFeignClients;
import org.springframework.context.ApplicationContext;

import java.io.IOException;
import java.math.BigInteger;
import java.text.DateFormat;
import java.util.Date;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import java.util.concurrent.TimeUnit;
import java.util.concurrent.atomic.AtomicReference;

@SpringBootApplication
@EnableDiscoveryClient
@EnableFeignClients
public class Main {


    public static void main(String[] args)
            throws IOException {
        ApplicationContext app = SpringApplication.run(Main.class, args);

//        System.setProperty("http.proxyHost", "localhost");
//        System.setProperty("http.proxyPort", "8888");

        //ServiceTest.test();


        //KvTest.test();
        DiscoveryClient discoveryClient = app.getBean(DiscoveryClient.class);

        List<ServiceInstance> list = discoveryClient.getInstances("distribution");

        System.in.read();
    }

}

class KvTest {

    public static void test()
            throws IOException {

        Consul consul = Consul.builder().withUrl("http://172.31.224.203:8500/").build(); // connect to Consul on localhost
        final KeyValueClient kvClient = consul.keyValueClient();
        String val = kvClient.getValueAsString("foo").get();
        System.out.println("Value is: " + val);

        ConsulResponseCallback<Optional<Value>> callback = new ConsulResponseCallback<Optional<Value>>() {

            AtomicReference<BigInteger> index = new AtomicReference<BigInteger>(null);

            @Override
            public void onComplete(ConsulResponse<Optional<Value>> consulResponse) {

                if (consulResponse.getResponse().isPresent()) {
                    Value v = consulResponse.getResponse().get();


                    System.out.println("Data:" + DateFormat.getDateTimeInstance().format(new Date()) + ", Value is: " + v.getValueAsString().get());
                }

                index.set(consulResponse.getIndex());
                watch();
            }

            void watch() {
                kvClient.getValue("" +
                        "foo", QueryOptions.blockMinutes(1, index.get()).build(), this);
            }

            @Override

            public void onFailure(Throwable throwable) {
                throwable.printStackTrace();
                watch();
            }
        };

        kvClient.getValue("foo", QueryOptions.blockMinutes(1, new BigInteger("0")).build(), callback);

    }

}

class ServiceTest {

    public static void test()
            throws IOException {

        String serviceName = "getUsers";
        Consul consul = Consul.builder().withUrl("http://172.31.224.203:8500/").build(); // connect to Consul on localhost

        HealthClient healthClient = consul.healthClient();

        List<ServiceHealth> nodes = healthClient.getHealthyServiceInstances(serviceName).getResponse(); // discover only "passing" nodes
        System.out.println(nodes.toString());


        ServiceHealthCache svHealth = ServiceHealthCache.newCache(healthClient, serviceName);
        svHealth.addListener(new ConsulCache.Listener<ServiceHealthKey, ServiceHealth>() {
            @Override
            public void notify(Map<ServiceHealthKey, ServiceHealth> newValues) {
                // do Something with updated server map

                System.out.println("Data:" + DateFormat.getDateTimeInstance().format(new Date()) + ", Value is: " + newValues.toString());
            }
        });

        svHealth.start();

    }

}
