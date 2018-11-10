class YouTube < Liquid::Tag
  Syntax = /^\s*([^\s]+)(\s+(\d+)\s+(\d+)\s*)?/

  def initialize(tagName, markup, tokens)
    super

    if markup =~ Syntax then
      @id = $1

      if $2.nil? then
          @width = 600
          @height = 600
      else
          @width = $2.to_i
          @height = $3.to_i
      end
    else
      raise "No ISF ID provided in the \"isf\" tag"
    end
  end

  def render(context)
    # "<iframe width=\"#{@width}\" height=\"#{@height}\" src=\"http://www.youtube.com/embed/#{@id}\" frameborder=\"0\"allowfullscreen></iframe>"
    "<iframe width=\"#{@width}\" height=\"#{@height}\" src=\"http://www.isf.video/sketches/#{@id}/embed\"></iframe>"
  end

  Liquid::Template.register_tag "isf", self
end