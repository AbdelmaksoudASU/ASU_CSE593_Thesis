FROM couchbase/server:community

COPY configure.sh /configure.sh

RUN sed -i -e 's/\r$//' configure.sh

CMD ["/configure.sh"]