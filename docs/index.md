## All Posts
{% for post in site.posts %}

### {{ post.title }}
<p>{{ post.description }}
<a href="{{ post.url | prepend: site.baseurl }}">Readmore..</a>
</p>
{% endfor %}



