# Generating a test certificate

* Install openssl.
* Generate private key: openssl genrsa 2048 > private.pem
* Generate the self signed certificate: openssl req -x509 -days 1000 -new -key private.pem -out public.pem
* If needed, create PFX: openssl pkcs12 -export -in public.pem -inkey private.pem -out mycert.pfx
<br> [discussion on stackoverflow](https://stackoverflow.com/questions/14267010/how-to-create-self-signed-ssl-certificate-for-test-purposes)