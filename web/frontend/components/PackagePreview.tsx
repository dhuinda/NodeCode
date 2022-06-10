import { Box, Flex, Heading, Img, Text } from '@chakra-ui/react'
import Link from 'next/link'
import { FC } from 'react'

interface Props {
  name: string
  version: string
  shortDescription: string
  isSmall?: boolean
}

const PackagePreview: FC<Props> = ({ name, version, shortDescription, isSmall = false }) => {
  const imgSize = isSmall ? '42px' : '56px'
  return (
    <Link href={`/${name}_v${version}`} passHref>
      <a>
        <Flex mt='10px' mr={{ sm: '0px', xl: '100px' }} border='2px solid #6e97b4' p='15px' w='100%' borderRadius='5px'>
          <Img src='/blue_cube.png' w={imgSize} h={imgSize}></Img>
          <Flex flexDir='column' justifyContent='center' ml='20px'>
            <Heading as='h6' fontWeight='normal' fontSize={isSmall ? '20px' : '24px'}>
              {name} v{version}
            </Heading>
            <Text fontSize='14px' color='gray'>
              {shortDescription}
            </Text>
          </Flex>
        </Flex>
      </a>
    </Link>
  )
}

export default PackagePreview
