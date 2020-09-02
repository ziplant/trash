<?php 

class Router {
	protected $path;

	function __construct() {
		$this->path = parse_url($_SERVER['REQUEST_URI']);
		$root = $_SERVER['PHP_SELF'];
		$root = substr_replace($root, '', strrpos($root, '/'));
		$this->path['path'] = '/'.ltrim($this->path['path'], $root);
	}																																																																																																																																											


	function route($url, $callback) {

		if (trim($this->path['path'], '/') === trim($url, '/')) {
			$callback($this->path['query']);																																																																																						
			exit();																			
		}

		if ($url === '404') {
			$callback();
			exit();
		}

		return;
	}
}

?>