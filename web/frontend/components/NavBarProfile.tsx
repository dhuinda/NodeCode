import { Icon, IconButton, Text } from '@chakra-ui/react'
import Cookies from 'js-cookie'
import Link from 'next/link'
import { useRouter } from 'next/router'
import { FC, useEffect, useState } from 'react'
import { FiUser } from 'react-icons/fi'

const NavBarProfile: FC = () => {
  const [isLoggedIn, setLoggedIn] = useState(false)
  const router = useRouter()

  const updateLoginStatus = async () => {
    try {
      const username = await fetch('/api/v1/users/username')
      if (await username.text()) {
        setLoggedIn(true)
      } else {
        setLoggedIn(false)
      }
    } catch {
      setLoggedIn(false)
    }
  }

  useEffect(() => {
    updateLoginStatus()
  }, [])

  if (isLoggedIn) {
    return <IconButton onClick={() => router.push('/account')} fontSize={24} aria-label='Account' icon={<FiUser />} />
  }
  return (
    <Link href='/api/v1/oauth2/code/github'>
      <a>
        <Text fontSize='12pt'>Sign in</Text>
      </a>
    </Link>
  )
}

export default NavBarProfile
